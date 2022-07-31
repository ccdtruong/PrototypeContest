using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float m_speed;

    [SerializeField]
    private float m_jumpForce;

    [SerializeField]
    private Transform m_groundCheckCollider;
    [SerializeField]
    LayerMask m_groundlayerMask;
    [SerializeField]
    LayerMask m_buttonlayerMask;


    private GameController m_controller;

    private float horizontal;

    private bool m_IsSelected;
    private bool m_isJumping;
    private bool m_isGrounded;

    private Vector3 m_screenBounds;

    private Animator m_animator;
    private Rigidbody2D m_rigidbody2D;
    private Vector2 m_lastVelocity;

    private float objectWidth;
    private float objectHeight;
    private float delta = 0.2f;


    private bool m_facingRight = true;

    private void Awake()
    {
        m_controller = GameObject.Find("GameController").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    public void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        m_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        m_isJumping = false;
        m_isGrounded = true;

        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    public void Update()
    {
        if (!m_IsSelected) return;
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        m_lastVelocity = m_rigidbody2D.velocity;
        //if(HoldTheButtonCheck())
        //{
        //    Debug.Log("Holding button");
        //    PlatformScript pls = GameObject.Find("Platform").GetComponent<PlatformScript>();
        //    pls.Trigger();
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!m_IsSelected) return;
        GroundCheck();
        //Debug.Log("isGround" + m_isGrounded);
        if (m_isGrounded)
        {
            m_animator.SetBool("isJumping", false);
            m_isJumping = false;
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
            m_isGrounded = false;
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

    public bool HoldTheButtonCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheckCollider.position, 0.2f, m_buttonlayerMask);
        if(colliders.Length > 0)
        {
            return true;
        }
        return false;
    }

    public void Move(float dir)
    {
        if (dir == 0)
        {
            m_animator.SetBool("isWalking", false);
            return;
        }
        else if ((dir < 0 && m_facingRight)
                || (dir > 0 && !m_facingRight))
        {
            Flip();
        }
        m_animator.SetBool("isWalking", true);
        float xVal = dir * m_speed * Time.fixedDeltaTime;
        Vector2 targetVelocity = new Vector2(xVal, m_rigidbody2D.velocity.y);
        m_rigidbody2D.velocity = targetVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            //GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
            //controller.OpenGate();
            m_controller.OpenGate();
            collision.gameObject.SetActive(false);
            Debug.Log("Get Key");
        }
        else if (collision.gameObject.tag == "Gate")
        {
            //GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
            //controller.PlayerPassTheGate(this.gameObject);
            m_controller.PlayerPassTheGate(this.gameObject);
        }
        else if(collision.gameObject.tag == "Punji")
        {
            m_controller.Heart--;
            Camera.main.GetComponent<CameraShake>().Shake();
        }
        else if(collision.gameObject.tag == "Gold")
        {
            m_controller.Coin += 10;
            collision.gameObject.SetActive(false);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var speed = m_lastVelocity.magnitude;
            var direction = Vector3.Reflect(m_lastVelocity.normalized, collision.contacts[0].normal);
            //m_rigidbody2D.velocity = (direction * Mathf.Max(speed, 0f) + new Vector3(0f, 5f));
            m_rigidbody2D.velocity = direction * Mathf.Max(speed + 2.5f, 0f);

        }
    }

    //prevent players from running out of the screen
    private void LateUpdate()
    {
        //GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
        //Vector3 bounds = controller.GetBounds();
        Vector3 bounds = m_controller.GetBounds();
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, bounds.x * -1 +  objectWidth - delta, bounds.x - objectWidth + delta);
        viewPos.y = Mathf.Clamp(viewPos.y, bounds.y * -1 + objectHeight - delta, bounds.y - objectHeight + delta);
        transform.position = viewPos;
    }

}
