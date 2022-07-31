using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Player m_grumpy;

    [SerializeField]
    private Player m_rebellious;


    private Vector3 m_screenBounds;
    private bool m_isGateOpened;


    private void Awake()
    {
        m_screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }
    void Start()
    {
        m_isGateOpened = false;
        m_grumpy.SetSelected(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Switch"))
        {
            SwitchCharacter();
        }
    }

    private void FixedUpdate()
    {
    }

    public Vector3 GetBounds()
    {
        return m_screenBounds;
    }

    private void SwitchCharacter()
    {
        if(m_grumpy.IsSelected())
        {
            m_rebellious.SetSelected(true);
            m_grumpy.SetSelected(false);
        }
        else
        {
            m_rebellious.SetSelected(false);
            m_grumpy.SetSelected(true);
        }
    }

    //public void OpenGate()
    //{
    //    m_isGateOpened = true;
    //}

    public void SetGateState(bool state)
    {
        m_isGateOpened = state;
    }

    public bool IsGateOpened()
    {
        return m_isGateOpened;
    }

    public void WinGame()
    {
        Debug.Log("WIN WIN WIN");
    }

    public void LoseGame()
    {
        Debug.Log("LOST LOST LOST");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
