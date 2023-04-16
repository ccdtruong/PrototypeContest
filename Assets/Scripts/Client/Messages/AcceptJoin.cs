using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AcceptJoin : MessageContent
{
    public AcceptJoin()
    {
        m_type = MessageType.AcceptJoin;
    }
}
