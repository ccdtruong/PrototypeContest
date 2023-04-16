using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public abstract class TCPConnection
{
    TCP tcp;
    protected string m_hostID;
    protected string m_playerID;
    public delegate void OnOpen();
    public event OnOpen m_OnOpen;
    protected TimeSpan m_ping;
    protected TCP.TCPType m_type;
    abstract protected void OnMessage<T>(Message<T> message) where T : MessageContent, new();
    abstract protected void OnMessage(Message<AcceptJoin> message);
    abstract protected void OnMessage(Message<Play> message);
    abstract protected void OnMessage(Message<SwitchCharacter> message);
    abstract protected void OnMessage(Message<JoinRequest> message);
    abstract protected void OnMessage(Message<Jump> message);
    abstract protected void OnMessage(Message<Move> message);
    abstract protected void OnMessage(Message<UpdatePlayerState> message);
    abstract protected void OnMessage(Message<Reset> message);
    abstract protected void OnMessage(Message<Quit> message);
    abstract protected void OnMessage(Message<Replay> message);

    public TCPConnection(string playerID, string hostID, TCP.TCPType type)
    {
        m_hostID = hostID;
        m_playerID = playerID;
        m_type = type;
        Start();
    }

    ~TCPConnection()
    {
        Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to ip: " + m_hostID);
        tcp = new TCP(m_type, m_hostID);

        tcp.OnOpen += () =>
        {
            Debug.Log("Connection opened!");
            m_OnOpen?.Invoke();
        };

        tcp.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        tcp.OnClose += () =>
        {
            Debug.Log("Connection closed!");
        };

        tcp.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);
            try
            {
                Message mess = JsonUtility.FromJson<Message>(message);

                // Only handle other player messages
                //if (mess.username == m_playerID) return;

                m_ping = (DateTime.UtcNow - mess.message.GetTimeStamp());

                switch (mess.message.GetMessageType())
                {
                    case MessageContent.MessageType.Move:
                        OnMessage(MessageFactory.CreateMessage<Move>(message));
                        break;
                    case MessageContent.MessageType.Jump:
                        OnMessage(MessageFactory.CreateMessage<Jump>(message));
                        break;
                    case MessageContent.MessageType.AcceptJoin:
                        OnMessage(MessageFactory.CreateMessage<AcceptJoin>(message));
                        break;
                    case MessageContent.MessageType.JoinRequest:
                        OnMessage(MessageFactory.CreateMessage<JoinRequest>(message));
                        break;
                    case MessageContent.MessageType.Play:
                        OnMessage(MessageFactory.CreateMessage<Play>(message));
                        break;
                    case MessageContent.MessageType.SwitchCharacter:
                        OnMessage(MessageFactory.CreateMessage<SwitchCharacter>(message));
                        break;
                    case MessageContent.MessageType.UpdatePlayerState:
                        OnMessage(MessageFactory.CreateMessage<UpdatePlayerState>(message));
                        break;
                    case MessageContent.MessageType.Reset:
                        OnMessage(MessageFactory.CreateMessage<Reset>(message));
                        break;
                    case MessageContent.MessageType.Quit:
                        OnMessage(MessageFactory.CreateMessage<Quit>(message));
                        break;
                    case MessageContent.MessageType.Replay:
                        OnMessage(MessageFactory.CreateMessage<Replay>(message));
                        break;
                    default:
                        Debug.Log("Unrecognized message!");
                        break;
                }
            } 
            catch (Exception e) 
            {
                Debug.Log(e);
            }
        };

        // waiting for messages
        tcp.Connect();
    }

    public void Close()
    {
        if (tcp != null) 
            tcp.Close();
    }

    public void Update()
    {
        tcp.Update();
    }

    public void SendMessage<T>(Message<T> message) where T : MessageContent, new()
    {
        if (IsOpen())
        {
            message.username = m_playerID;
            Debug.Log("Message sent: " + message.ToString());
            tcp.SendText(message.ToString());
        }
    }

    public string GetHostID()
    {
        return m_hostID;
    }
    public string GetPlayerID()
    {
        return m_playerID;
    }

    public bool IsOpen()
    {
        return tcp.IsOpen();
    }

    public double GetPing()
    {
        return Math.Abs(m_ping.TotalMilliseconds);
    }
}

