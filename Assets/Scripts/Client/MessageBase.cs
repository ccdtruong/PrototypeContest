using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

[Serializable]
public class Message<T> where T : MessageContent, new()
{
    public string username;
    public T message;

    public Message()
    {
        message = new T();
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}

[Serializable]
public class Message : Message<MessageContent>
{
}

[Serializable]
public class MessageContent
{
    public enum MessageType
    {
        JoinRequest,
        AcceptJoin,
        SwitchCharacter,
        Play,
        Move,
        Jump,
        UpdatePlayerState,
        Reset,
        Quit,
        Replay
    }

    [SerializeField]
    protected MessageType m_type;
    [SerializeField]
    protected string m_timeStamp;

    public MessageContent()
    {
        //Debug.Log("init Datetime");
        var test = DateTime.UtcNow;
        m_timeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss:fffffZ");
        //Debug.Log(test.ToString() + " " + DateTime.ParseExact(m_timeStamp, "yyyy-MM-ddTHH:mm:ss:fffffZ", null).ToUniversalTime().ToString());
    }

    public MessageType GetMessageType()
    {
        return m_type;
    }

    public DateTime GetTimeStamp()
    {
        Debug.Log(m_timeStamp);
        return DateTime.ParseExact(m_timeStamp, "yyyy-MM-ddTHH:mm:ss:fffffZ", null).ToUniversalTime();
    }
}


public class MessageFactory
{
    static public Message<T> CreateMessage<T>(string message) where T : MessageContent, new()
    {
        return JsonUtility.FromJson<Message<T>>(message);
    }
}

