using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCheck : MonoBehaviour
{
    private GameController m_controller;

    private void Awake()
    {
        m_controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "RigidObjects")
        {
            Camera.main.GetComponent<CameraShake>().Shake();
            SoundManager.Instance.Play("collide");
            m_controller.Heart--;
        }
    }
}
