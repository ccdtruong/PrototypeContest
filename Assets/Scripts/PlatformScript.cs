using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PLATFORMTYPE
{ 
    UP, DOWN, LEFT, RIGHT
}

public class PlatformScript : MonoBehaviour
{
    [SerializeField]
    private Transform m_startPoint;

    [SerializeField]
    private Transform m_endPoint;

    [SerializeField] private float m_speed;

    private bool m_isActive;

    [SerializeField] private PLATFORMTYPE m_platformType;
    public void Trigger()
    {
        m_isActive = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        //m_isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_isActive)
        {
            return;
        }
        if(m_platformType == PLATFORMTYPE.UP)
        {
            if(transform.position.y >= m_endPoint.position.y)
            {
                m_isActive = false;
                return;
            }
            Vector2 pos = transform.position;
            pos.y += m_speed * Time.deltaTime;
            transform.position = pos;
        }
        else if(m_platformType == PLATFORMTYPE.DOWN)
        {
            if (transform.position.y <= m_endPoint.position.y)
            {
                m_isActive = false;
                return;
            }
            Vector2 pos = transform.position;
            pos.y -= m_speed * Time.deltaTime;
            transform.position = pos;
        }
    }
}
