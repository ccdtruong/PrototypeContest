using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Jump : MessageContent
{
    public Jump()
    {
        m_type = MessageType.Jump;
    }
}
