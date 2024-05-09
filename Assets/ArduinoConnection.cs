using UnityEngine;
using System.IO.Ports;
using System;
using UnityEditor.Experimental.GraphView;

public class ArduinoConnection : MonoBehaviour
{
    public string port;
    SerialPort sp;
    bool isStreaming = false;

    //To Read info from sensor
    [HideInInspector] public string data;

    private void Start()
    {
        sp = new SerialPort(port, 9600);

        OpenConnection();
    }

    private void OnDestroy()
    {
        CloseConnection();
    }

    void OpenConnection()
    {
        isStreaming = true;
        sp.ReadTimeout = 100;
        sp.Open();

        Debug.Log("Port opened!");
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
            message = sp.ReadLine();
            return message;
        }
        catch (TimeoutException)
        {
            return null;
        }

    }
    
    void Update()
    {
        if (isStreaming)
        {
            string value = ReadSerialPort();
            
            Debug.Log(value);

            /*if (value != null)
            {
            }*/
        }
    }
}