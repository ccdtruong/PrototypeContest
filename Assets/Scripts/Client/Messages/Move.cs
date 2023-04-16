using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Move : MessageContent
{
    public float horizontal;

    public Move()
    {
        m_type = MessageType.Move; 
    }
}

