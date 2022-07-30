using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] PlatformScript m_platform;
    private SpriteRenderer m_spriteRenderer;
    private Collider2D m_collider;
    private const float m_releaseOffset = -0.3f;
    private const float m_pressOffset = -0.45f;
    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<Collider2D>();
        OnRelease();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPressed()
    {
        m_spriteRenderer.sprite = Resources.Load<Sprite>("Environment/platformPack_tile063");
        m_collider.offset = new Vector2(0f, m_pressOffset);

    }

    public void OnRelease()
    {
        m_spriteRenderer.sprite = Resources.Load<Sprite>("Environment/platformPack_tile062");
        m_collider.offset = new Vector2(0f, m_releaseOffset);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.tag == "Player")
    //    {
    //        m_platform.Trigger();
    //    }
    //}
}
