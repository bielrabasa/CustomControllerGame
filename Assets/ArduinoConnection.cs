using UnityEngine;
using System.IO.Ports;
using System;
using UnityEditor.Experimental.GraphView;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using TMPro;

public class ArduinoConnection : MonoBehaviour
{
    string port;

    SerialPort sp;
    bool isStreaming = false;

    Thread readingThread;

    //To Read info from sensor
    [HideInInspector] public Vector3 acceleration = Vector3.forward;
    [HideInInspector] public Vector3 rotation;
    [HideInInspector] public Vector3 lastRotation;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if(FindObjectsOfType<ArduinoConnection>().Length > 1) Destroy(gameObject);
    }

    public void SetPayload()
    {
        port = FindObjectOfType<TMP_InputField>().text;
        StartConnection();
    }

    private void StartConnection()
    {
        sp = new SerialPort(port, 9600);

        OpenConnection();

        if (isStreaming)
        {
            readingThread = new Thread(ContinuousReading);
            readingThread.Start();
        }

        SceneManager.LoadScene("MainScene");
    }

    private void Update()
    {
        if (isStreaming)
        {
            if (Mathf.Abs(rotation.z - lastRotation.z) > 1.5f) FindObjectOfType<BaseFlight>()?.RollPrepare();
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Return) && SceneManager.GetActiveScene().name == "IntroScreen") 
        {
            SetPayload();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) FindObjectOfType<BaseFlight>()?.RollPrepare();

        acceleration = Vector3.forward;
        if (Input.GetKey(KeyCode.RightArrow)) acceleration.x--;
        if (Input.GetKey(KeyCode.LeftArrow)) acceleration.x++;
        if (Input.GetKey(KeyCode.UpArrow)) acceleration.y--;
        if (Input.GetKey(KeyCode.DownArrow)) acceleration.y++;
        acceleration.Normalize();
    }

    private void OnDestroy()
    {
        CloseConnection();
    }

    void OpenConnection()
    {
        try
        {
            sp.ReadTimeout = 100;
            sp.Open();
            isStreaming = true;
            Debug.Log("Port opened!");
        }
        catch
        {
            Debug.Log(port + " not connected, keyboard mode connected!");
        }
    }

    void CloseConnection()
    {
        if(sp != null) sp.Close();
    }

    string ReadSerialPort()
    {
        string message;

        try
        {
            sp.ReadTimeout = 150;
            message = sp.ReadLine();
            return message;
        }
        catch (TimeoutException)
        {
            return null;
        }

    }
    
    void ContinuousReading()
    {
        Debug.Log("Reading started!");

        while(isStreaming)
        {
            string data = null;
            try
            {
                data = ReadSerialPort();
            }
            catch
            {
                Debug.Log("Thread stopped!");
                isStreaming = false;
            }
            
            //Parse data to acc vector
            if(data != null)
            {
                string[] d = data.Split(',');
                if (d.Length != 8) continue;

                if (d[0] == "Acceleration")
                {
                    acceleration = new Vector3(float.Parse(d[1]), float.Parse(d[2]), float.Parse(d[3])).normalized;
                }

                if (d[4] == "Rotation")
                {
                    lastRotation = rotation;;
                    rotation = new Vector3(float.Parse(d[5]), float.Parse(d[6]), float.Parse(d[7])).normalized;
                }
            }
        }
    }

    public void checkVibration(string preFix)
    {
        bool vibration = true;

        sp.WriteLine(preFix + (vibration ? "1" : "0" ));
    }
}