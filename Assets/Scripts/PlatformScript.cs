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
    private bool m_isReached;
    private Vector3 m_initEndPos;
    private Vector3 m_initStartPos;

    [SerializeField] private PLATFORMTYPE m_platformType;
    public void Activate()
    {
        m_isActive = true;
        m_isReached = false;
    }

    public void Deactivate()
    {
        m_isActive = false;
        m_isReached = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_initEndPos = m_endPoint.position;
        m_initStartPos = m_startPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_isActive)
        {
            MoveToStart();
            return;
        }
        MoveToEnd();
    }

    void MoveToEnd()
    {
        Debug.Log("Move End");
        if (m_isReached)
            return;

        if (m_platformType == PLATFORMTYPE.UP)
        {
            if (transform.position.y >= m_initEndPos.y)
            {
                Debug.Log("Move End Reach");
                m_isReached = true;
                return;
            }
            Debug.Log("Move End");
            Vector3 pos = transform.position;
            pos.y += m_speed * Time.deltaTime;
            transform.position = pos;
        }
        else if (m_platformType == PLATFORMTYPE.DOWN)
        {
            if (transform.position.y <= m_initEndPos.y)
            {
                m_isReached = true;
                return;
            }
            Vector3 pos = transform.position;
            pos.y -= m_speed * Time.deltaTime;
            transform.position = pos;
        }
        m_isActive = false;
    }
    
    void MoveToStart()
    {
        if (m_isReached)
            return;

        if (m_platformType == PLATFORMTYPE.UP)
        {
            if (transform.position.y <= m_initStartPos.y)
            {
                m_isReached = true;
                return;
            }
            Vector3 pos = transform.position;
            pos.y -= m_speed * Time.deltaTime;
            transform.position = pos;
        }
        else if (m_platformType == PLATFORMTYPE.DOWN)
        {
            if (transform.position.y >= m_initStartPos.y)
            {
                m_isReached = true;
                return;
            }
            Vector3 pos = transform.position;
            pos.y += m_speed * Time.deltaTime;
            transform.position = pos;
        }
    }
}
