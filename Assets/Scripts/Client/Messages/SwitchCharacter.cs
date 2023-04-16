using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SwitchCharacter : MessageContent
{
    public SwitchCharacter()
    {
        m_type = MessageType.SwitchCharacter;
    }
}
