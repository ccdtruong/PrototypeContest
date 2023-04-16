using System.Collections;
using UnityEngine;
using System.Net.Sockets;

public class TCP
{
    public enum TCPType
    {
        Client,
        Server
    }

    TCPClient m_client;
    TCPServer m_server;
    TCPType m_type;
    volatile bool m_isOpen = false;

    public delegate void OnOpenFunc();
    public event OnOpenFunc OnOpen;
    public delegate void OnErrorFunc(SocketException e);
    public event OnErrorFunc OnError;
    public delegate void OnCloseFunc();
    public event OnCloseFunc OnClose;
    public delegate void OnMessageFunc(byte[] bytes);
    public event OnMessageFunc OnMessage;

    public TCP(TCPType type, string host)
    {
        m_type = type;
        if (m_type == TCPType.Client)
        {
            m_client = new TCPClient(host);
            m_client.OnOpen += () =>
            {
                m_isOpen = true;
                OnOpen?.Invoke();
            };
            m_client.OnError += (e) =>
            {
                OnError?.Invoke(e);
            };
            m_client.OnClose += () =>
            {
                OnClose?.Invoke();
            };
            m_client.OnMessage += (bytes) =>
            {
                OnMessage?.Invoke(bytes);
            };
        }
        if (m_type == TCPType.Server)
        {
            m_server = new TCPServer(host);
            m_server.OnOpen += () =>
            {
                m_isOpen = true;
                OnOpen?.Invoke();
            };
            m_server.OnError += (e) =>
            {
                OnError?.Invoke(e);
            };
            m_server.OnClose += () =>
            {
                OnClose?.Invoke();
            };
            m_server.OnMessage += (bytes) =>
            {
                OnMessage?.Invoke(bytes);
            };
        }
    }

    public void Connect()
    {
        if (m_type == TCPType.Client) m_client.ConnectToTcpServer();
        if (m_type == TCPType.Server) m_server.StartListening();
    }

    public void Update()
    {
        if (m_type == TCPType.Client) m_client.Update();
        if (m_type == TCPType.Server) m_server.Update();
    }

    public void Close()
    {
        if (m_type == TCPType.Client) m_client.Close();
        if (m_type == TCPType.Server) m_server.Close();
    }

    public bool IsOpen()
    {
        Debug.Log("IsOpen " + m_isOpen);
        return m_isOpen;
    }

    public void SendText(string text)
    {
        if (m_type == TCPType.Client) m_client.SendMessage(text);
        if (m_type == TCPType.Server) m_server.SendMessage(text);
    }
}