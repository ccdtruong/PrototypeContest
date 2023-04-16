using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : TCPConnection
{
    public delegate void OnMove(float hor);
    public event OnMove m_OnMove;
    public delegate void OnJump();
    public event OnJump m_OnJump;
    public delegate void OnUpdatePlayerState(Message<UpdatePlayerState> message);
    public event OnUpdatePlayerState m_OnUpdatePlayerState;
    public delegate void OnReset();
    public event OnReset m_OnReset;
    public delegate void OnQuit();
    public event OnQuit m_OnQuit;
    public delegate void OnReplay(int level);
    public event OnReplay m_OnReplay;

    public Client(string playerID, string hostID, TCP.TCPType type) : base(playerID, hostID, type)
    {
    }
    protected override void OnMessage<T>(Message<T> message)
    {
        Debug.Log("Unhandled Message");
    }

    protected override void OnMessage(Message<Move> message) 
    {
        Debug.Log("Move Message");
        m_OnMove?.Invoke(message.message.horizontal);
    }

    protected override void OnMessage(Message<Jump> message)
    {
        Debug.Log("Jump Message");
        m_OnJump?.Invoke();
    }

    protected override void OnMessage(Message<Play> message)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnMessage(Message<SwitchCharacter> message)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnMessage(Message<JoinRequest> message)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnMessage(Message<AcceptJoin> message)
    {
        throw new System.NotImplementedException();
    }

    public void Move(float hor)
    {
        Message<Move> mess = new Message<Move>();
        mess.message.horizontal = hor;
        SendMessage(mess);
    }

    public void Jump()
    {
        Message<Jump> mess = new Message<Jump>();
        SendMessage(mess);
    }

    public void UpdatePlayerState(GameSettings.Character character, Vector2 position, bool facingRight)
    {
        Message<UpdatePlayerState> mess = new Message<UpdatePlayerState>();
        mess.message.position = position;
        mess.message.facingRight = facingRight;
        mess.message.character = character;
        SendMessage(mess);
    }

    public void Reset()
    {
        Message<Reset> mess = new Message<Reset>();
        SendMessage(mess);
    }

    public void Quit()
    {
        Message<Quit> mess = new Message<Quit>();
        SendMessage(mess);
    }

    public void Replay(int level = 0)
    {
        Message<Replay> mess = new Message<Replay>();
        mess.message.level = level;
        SendMessage(mess);
    }

    protected override void OnMessage(Message<UpdatePlayerState> message)
    {
        Debug.Log("UpdatePlayerState Message");
        m_OnUpdatePlayerState?.Invoke(message);
    }

    protected override void OnMessage(Message<Reset> message)
    {
        Debug.Log("Reset Message");
        m_OnReset?.Invoke();
    }

    protected override void OnMessage(Message<Quit> message)
    {
        Debug.Log("Quit Message");
        m_OnQuit?.Invoke();
    }

    protected override void OnMessage(Message<Replay> message)
    {
        Debug.Log("Replay Message");
        m_OnReplay?.Invoke(message.message.level);
    }
}
