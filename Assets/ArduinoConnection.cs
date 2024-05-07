using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoConnection : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM3", 9600);
    bool isStreaming = false;

    private void Start()
    {
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
    }

    void CloseConnection()
    {
        sp.Close();
    }

    public string ReadSerialPort(int timeout = 50)
    {
        if (!isStreaming) return null;

        string message;
        sp.ReadTimeout = timeout;

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
    /*void Update()
    {
        if (isStreaming)
        {
            string value = ReadSerialPort();
            if (value != null)
            {
                Debug.Log(value);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchLEDState();
            }
        }
    }*/

    //__________________________________

    /*bool ledOn = false;

    void SwitchLEDState()
    {
        ledOn = !ledOn;
        sp.WriteLine("L" + (ledOn ? "1" : "0"));
    }*/
}