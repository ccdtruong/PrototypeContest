using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant : Client
{
    public delegate void OnPlay();
    public event OnPlay m_OnPlay;
    public delegate void OnSwitchCharacter();
    public event OnSwitchCharacter m_OnSwitchCharacter;
    public delegate void OnAcceptJoin();
    public event OnAcceptJoin m_OnAcceptJoin;

    public Participant(string playerID, string hostID) : base(playerID, hostID, TCP.TCPType.Client)
    {
    }

    public void JoinRequest()
    {
        Message<JoinRequest> mess = new Message<JoinRequest>();
        SendMessage(mess);
    }

    protected override void OnMessage(Message<AcceptJoin> message)
    {
        Debug.Log("Accept Join Message Received");
        //if (message.username == m_hostID)
        //{
            m_OnAcceptJoin?.Invoke();
        //}
    }

    protected override void OnMessage(Message<SwitchCharacter> message)
    {
        Debug.Log("Switch Char Mess Received");
        m_OnSwitchCharacter?.Invoke();
    }

    protected override void OnMessage(Message<Play> message)
    {
        Debug.Log("Play Mess Received");
        m_OnPlay?.Invoke();
    }
}
