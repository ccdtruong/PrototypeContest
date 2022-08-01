using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer;
    [SerializeField] PlatformScript m_platform;
    [SerializeField] Sprite m_activeSprite;
    Sprite m_deactiveSprite;

    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_deactiveSprite = m_spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "RigidObjects")
        {
            m_platform.Activate();
            m_spriteRenderer.sprite = m_activeSprite;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "RigidObjects")
        {
            m_platform.Deactivate();
            m_spriteRenderer.sprite = m_deactiveSprite;
        }
    }
}
