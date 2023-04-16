using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Play : MessageContent
{
    public Play()
    {
        m_type = MessageType.Play;
    }
}
