using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UpdatePlayerState : MessageContent
{
    public Vector2 position;
    public bool facingRight;
    public GameSettings.Character character;

    public UpdatePlayerState()
    {
        m_type = MessageType.UpdatePlayerState; 
    }
}

