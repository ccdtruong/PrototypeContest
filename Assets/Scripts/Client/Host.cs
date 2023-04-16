using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Host : Client
{
    public delegate void OnJoinRequest(Message<JoinRequest> message);
    public event OnJoinRequest m_OnJoinRequest;

    public Host(string playerID, string hostID) : base(playerID, hostID, TCP.TCPType.Server)
    {
    }

    public void AcceptJoin()
    {
        Message<AcceptJoin> mess = new Message<AcceptJoin>();
        SendMessage(mess);
    }

    public void SwitchCharacter()
    {
        Message<SwitchCharacter> mess = new Message<SwitchCharacter>();
        SendMessage(mess);
    }

    public void Play()
    {
        Message<Play> mess = new Message<Play>();
        SendMessage(mess);
    }

    protected override void OnMessage(Message<JoinRequest> message)
    {
        Debug.Log("Join Request Message Received");
        m_OnJoinRequest?.Invoke(message);
    }
}
