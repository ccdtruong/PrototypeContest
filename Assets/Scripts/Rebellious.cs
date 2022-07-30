using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebellious : Player 
{
    private Rigidbody2D m_rgb;
    private Vector3 m_lastVelocity;

    public void Start()
    {
        base.Start();
        m_rgb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        base.Update();
        m_lastVelocity = m_rgb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var speed = m_lastVelocity.magnitude;
            var direction = Vector3.Reflect(m_lastVelocity.normalized, collision.contacts[0].normal);
            m_rgb.velocity = direction * Mathf.Max(speed + 1f, 0f);
        }
    }
}
