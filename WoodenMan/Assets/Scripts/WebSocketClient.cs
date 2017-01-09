using System;
using UnityEngine;
using WebSocketSharp;

public class WebSocketClient
{
    WebSocketSharp.WebSocket m_ws;
    public string m_Message;
    bool m_isWSConnected = false;

    // Connect WebSocket
    void ConnectWS()
    {
        if (m_isWSConnected)
            return;

        m_Message = "No data.";

        //using (m_ws = new WebSocket("ws://localhost:8080/"))
        using (m_ws = new WebSocket("ws://139.19.40.35:8080/"))
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

    // Use this for initialization
    public void Start()
    {
        ConnectWS();
    }

    // Update is called once per frame
    public void Update()
    {
        //Debug.Log(m_Message);
        //// Parse message
        //if (m_InputParser != null)
        //    m_InputParser.parse(m_Message);
    }

    public void OnApplicationQuit()
    {
        if (m_ws != null && m_ws.ReadyState == WebSocketState.OPEN)
            m_ws.Close();
    }
}