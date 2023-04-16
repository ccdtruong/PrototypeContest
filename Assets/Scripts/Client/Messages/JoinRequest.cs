using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class JoinRequest : MessageContent
{
    public JoinRequest()
    {
        m_type = MessageType.JoinRequest;
    }
}
