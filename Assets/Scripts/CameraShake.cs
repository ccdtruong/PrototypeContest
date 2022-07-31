using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float m_duration = 1f;
    [SerializeField] AnimationCurve curve;
    private bool m_isShaking;
    private float m_elapsedTime = 0f;
    private Vector3 m_startPos;

    public void Shake()
    {
        m_isShaking = true;
    }

    private void Start()
    {
        m_isShaking = false;
        m_startPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (!m_isShaking)
        {
            return;
        }
        else
        {
            if (m_elapsedTime < m_duration)
            {
                m_elapsedTime += Time.deltaTime;
                float strength = curve.Evaluate(m_elapsedTime/m_duration);
                transform.position = m_startPos + Random.insideUnitSphere * strength;
            }
            else
            {
                m_isShaking = false;
                transform.position = m_startPos;
                m_elapsedTime = 0f;
            }
        }
    }
}
