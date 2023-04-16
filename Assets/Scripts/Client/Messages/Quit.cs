using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Quit : MessageContent
{
    public Quit()
    {
        m_type = MessageType.Quit;
    }
}
