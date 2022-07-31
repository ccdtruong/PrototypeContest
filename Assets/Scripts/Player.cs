using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float m_speed;

    [SerializeField]
    private float m_jumpSpeed;

    [SerializeField]
    private float m_jumpForce;

    [SerializeField]
    private Transform m_groundCheckCollider;
    
    [SerializeField]
    LayerMask m_groundlayerMask;

    [SerializeField]
    ParticleSystem m_sparksParticles;

    private float horizontal;
    private bool m_IsSelected;
    private bool m_isJumping;
    private bool m_isGrounded;
    private bool m_isSparking;
    private Vector3 m_screenBounds;

    private Animator m_animator;
    private Rigidbody2D m_rigidbody2D;


    private bool m_facingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        m_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        m_isJumping = false;
        m_isGrounded = true;
    }

    private void Update()
    {
        if (!m_IsSelected) return;
        horizontal = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        //Debug.Log("isJumping = " + m_isJumping);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!m_IsSelected) return;
        GroundCheck();
        Debug.Log("isGround = " + m_isGrounded);
        if (m_isGrounded)
        {
            m_animator.SetBool("isJumping", false);
            m_isJumping = false;
            Debug.Log("isJumpingFalse");
        } else
        {
            Debug.Log("isJumpingTrue " + m_isJumping);
            if (m_isJumping) m_animator.SetBool("isJumping", true);
        }
        Move(horizontal);
    }

    public void SetSelected(bool isSelected)
    {
        m_IsSelected = isSelected;
    }

    public bool IsSelected()
    {
        return m_IsSelected;
    }

    public void Jump()
    {
        if (!m_isJumping && m_isGrounded)
        {
            m_animator.SetBool("isJumping", true);
            m_rigidbody2D.velocity = Vector2.up * m_jumpForce;
            m_isJumping = true;
        }
    }

    private void Flip()
    {
        m_facingRight = !m_facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void GroundCheck()
    {
        m_isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheckCollider.position, 0.1f, m_groundlayerMask);
        //Debug.Log("colliders.Length = " + colliders.Length);
        if (colliders.Length > 0)
        {
            m_isGrounded = true;
        }
    }

    public void Move(float dir)
    {
        if (dir == 0 || m_isSparking)
        {
            m_animator.SetBool("isWalking", false);
            return;
        }
        else if ((dir < 0 && m_facingRight)
                || (dir > 0 && !m_facingRight))
        {
            Flip();
        }
        if (m_isGrounded)
        {
            m_animator.SetBool("isWalking", true);
        }
        float speed = m_speed;
        if (!m_isGrounded)
        {
            speed = m_jumpSpeed;
        }
        float xVal = dir * speed * Time.fixedDeltaTime;
        Vector2 targetVelocity = new Vector2(xVal, m_rigidbody2D.velocity.y);
        m_rigidbody2D.velocity = targetVelocity;
        //Debug.Log("speed " + name + " = " + m_rigidbody2D.velocity.ToString());
    }

    void SparkBurst()
    {
        m_sparksParticles.Emit(20);
    }

    void SparkStop()
    {
        //Debug.Log("End spark");
        m_animator.SetBool("isFocus", false);
        m_isSparking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
            //controller.OpenGate();
            controller.SetGateState(true);
            collision.gameObject.SetActive(false);
            //Debug.Log("Get Key");
        }
        else if (collision.gameObject.tag == "Gate")
        {
            GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
            controller.WinGame();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 posA = transform.position;
            Vector2 posB = collision.gameObject.transform.position;
            Vector2 collisionVector = posB - posA;
            m_rigidbody2D.velocity = - collisionVector * m_jumpForce;
            Debug.Log("collisionVecter " + name + " = " + m_rigidbody2D.velocity.ToString());
            if ((collisionVector.x < 0 && m_facingRight)
                || (collisionVector.x > 0 && !m_facingRight))
            {
                Flip();
            }
            SparkBurst();
            m_isSparking = true;
            m_animator.SetBool("isFocus", true);
            Invoke("SparkStop", 0.3f);
        }
    }
}
