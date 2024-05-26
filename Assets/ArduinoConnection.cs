using UnityEngine;
using System.IO.Ports;
using System;
using UnityEditor.Experimental.GraphView;
using System.Threading;

public class ArduinoConnection : MonoBehaviour
{
    public string port;
    SerialPort sp;
    bool isStreaming = false;

    Thread readingThread;

    //To Read info from sensor
    [HideInInspector] public Vector3 acceleration = Vector3.forward;

    private void Start()
    {
        sp = new SerialPort(port, 9600);
        
        OpenConnection();

        if (isStreaming)
        {
            readingThread = new Thread(ContinuousReading);
            readingThread.Start();
        }
    }

    private void Update()
    {
        if (isStreaming) return;

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
        sp.Close();
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
                if (d[0] == "Acceleration")
                {
                    acceleration = new Vector3(float.Parse(d[1]), float.Parse(d[2]), float.Parse(d[3])).normalized;
                }
            }
        }
    }
}