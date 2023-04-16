using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grumpy : Player
{
    public new void Start()
    {
        m_char = GameSettings.Character.Grumpy;
        base.Start();
    }
}
