using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public abstract class Connection
{
    WebSocket websocket;
    protected string m_hostID;
    protected string m_playerID;
    //Queue<object> m_messageQueue;
    public delegate void OnOpen();
    public event OnOpen m_OnOpen;
    protected TimeSpan m_ping;
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

    public Connection(string playerID, string hostID)
    {
        m_hostID = hostID;
        m_playerID = playerID;
        Start();
    }

    ~Connection()
    {
        Close();
    }

    // Start is called before the first frame update
    async void Start()
    {
        var byteArray = Convert.FromBase64String(m_hostID + "==");
        var guid = new Guid(byteArray);
        Debug.Log("Connecting to room: " + guid.ToString("N"));
        //websocket = new WebSocket("wss://kychatserver.herokuapp.com/ws/chat/" + guid.ToString("N") + "/");


        websocket.OnOpen += () =>
        {
            Debug.Log("Connection opened!");
            m_OnOpen?.Invoke();
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed! " + e);
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            //Debug.Log("OnMessage! " + message);
            Message mess = JsonUtility.FromJson<Message>(message);

            // Only handle other player messages
            if (mess.username == m_playerID) return;

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
        };

        // waiting for messages
        await websocket.Connect();
    }

    async public void Close()
    {
        if (websocket != null) 
            await websocket.Close();
    }

    public void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    public async void SendMessage<T>(Message<T> message) where T : MessageContent, new()
    {
        if (IsOpen())
        {
            message.username = m_playerID;
            Debug.Log("Message sent: " + message.ToString());
            await websocket.SendText(message.ToString());
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
        return websocket.State == WebSocketState.Open;
    }

    public double GetPing()
    {
        return Math.Abs(m_ping.TotalMilliseconds);
    }
}

