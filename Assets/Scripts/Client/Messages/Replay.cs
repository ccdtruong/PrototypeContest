using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Replay : MessageContent
{
    public int level;

    public Replay()
    {
        m_type = MessageType.Replay;
    }
}
