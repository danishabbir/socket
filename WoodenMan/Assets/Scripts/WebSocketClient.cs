using System;
using UnityEngine;
using WebSocketSharp;

public class WebSocketClient : MonoBehaviour
{
    WebSocketSharp.WebSocket m_ws;
    public string m_Message;
    bool m_isWSConnected = false;

    private bool m_isKFWMode = true;
    private runLive m_AnyMethod;

    void ConnectWS()
    {
        if (m_isWSConnected)
            return;

        m_Message = "No data.";

        //using (m_ws = new WebSocket("ws://localhost:8080/"))
        //using (m_ws = new WebSocket("ws://139.19.111.138:8080/")) // Dell big machine
        using (m_ws = new WebSocket("ws://139.19.111.107:8080/")) // Red machine
        {
            //m_ws.Log.Level = WebSocketSharp.LogLevel.TRACE;
            //m_ws.Log.File = "D:\\ws_log.txt";

            m_ws.OnOpen += (sender, e) =>
            {
                m_ws.Send(String.Format("Hello server."));
                Debug.Log("Connection opened.");
                m_isWSConnected = true;
            };
            m_ws.OnMessage += (sender, e) =>
            {
                m_Message = e.Data;
                //Debug.Log(m_Message);
            };
            m_ws.OnClose += (sender, e) =>
            {
                m_ws.Connect(); // This is a hack, but whatever
                m_isWSConnected = false;
            };
            m_ws.OnError += (sender, e) =>
            {
                // NOT PRINTING ERRORS
                //Debug.LogError(e.Message);
                m_isWSConnected = false;
            };

            m_ws.Connect();
        }
    }

    public void Start()
    {
        //Screen.SetResolution(1920, 1080, true);
        Application.runInBackground = true;

        m_isKFWMode = true;
        ConnectWS();

        if (m_isKFWMode)
            m_AnyMethod = new runLiveKFW();
        else
            m_AnyMethod = new runLiveVNect();

        if (gameObject.name == "FollowerCamera")
            m_AnyMethod.m_isVRMode = false;
        else
        {
            Debug.Log("Running in VR mode.");
            m_AnyMethod.m_isVRMode = true;
        }

        m_AnyMethod.Start();
    }

    public void Update()
    {
        //Debug.Log(m_Message);
        if (m_AnyMethod == null)
            return;

        m_AnyMethod.Update(m_Message);
    }

    public void OnApplicationQuit()
    {
        if (m_ws != null && m_ws.ReadyState == WebSocketState.OPEN)
            m_ws.Close();
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(Screen.currentResolution.width / 2, Screen.currentResolution.height / 20, Screen.currentResolution.width / 15, Screen.currentResolution.height / 20), "Recenter"))
        {
            Debug.Log("Recentering VR.");
            if (m_AnyMethod != null)
                m_AnyMethod.recenter();
        }

        Event e = Event.current;
        if (e.isKey)
        {
            //Debug.Log("Detected key code: " + e.keyCode);
            if(e.keyCode == KeyCode.R)
            {
                Debug.Log("Recentering VR.");
                if (m_AnyMethod != null)
                    m_AnyMethod.recenter();
            }
        }
    }
}