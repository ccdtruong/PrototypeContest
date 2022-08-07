using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBox : MonoBehaviour
{

    [SerializeField]
    ParticleSystem m_fireParticles;
    [SerializeField]
    LayerMask m_groundlayerMask;
    [SerializeField]
    float m_maxYVelocity;

    private bool m_isGrounded;
    private bool m_isExploded;
    private Rigidbody2D m_rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        m_isGrounded = true;
        m_isExploded = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GroundCheck();
        ExplosionCheck();
        if (m_isGrounded && m_isExploded)
        {
            var dur = m_fireParticles.main.duration;
            m_fireParticles.Play();
            SoundManager.Instance.Play("explosion");
            Camera.main.GetComponent<CameraShake>().Shake();
            Invoke("DestroyObject", dur);
        }
    }

    void GroundCheck()
    {
        m_isGrounded = false;
        Transform boxTransform = GetComponent<BoxCollider2D>().transform;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxTransform.position, boxTransform.localScale, boxTransform.eulerAngles.z, m_groundlayerMask);
        // Subtract 1 because a box is also a ground
        if (colliders.Length - 1 > 0)
        {
            m_isGrounded = true;
        }
    }

    void ExplosionCheck()
    {
        Debug.Log("v = " + m_rigidbody2D.velocity.y);
        if (Mathf.Abs(m_rigidbody2D.velocity.y) >= m_maxYVelocity)
        {
            m_isExploded = true;
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
        GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
        controller.Heart--;
    }
}
