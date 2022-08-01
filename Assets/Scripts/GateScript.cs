using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_topGate;
    private SpriteRenderer m_bottomGate;

    [SerializeField]
    private Sprite m_spriteTopGateOpen;
    [SerializeField]
    private Sprite m_spriteBottomGateOpen;

    private void Start()
    {
        m_bottomGate = GetComponent<SpriteRenderer>();
    }

    public void OpenGate()
    {
        m_topGate.sprite = m_spriteTopGateOpen;
        m_bottomGate.sprite = m_spriteBottomGateOpen;
    }
}
