using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Player m_grumpy;

    [SerializeField]
    private Player m_rebellious;

    private Vector3 m_screenBounds;
    private bool m_isGateOpened;
    private int m_playerPassed;


    [SerializeField] private Text heartLabel;
    private int heart;
    public int Heart
    {
        get { return heart; }
        set 
        { 
            heart = value;
            heartLabel.GetComponent<Text>().text = "x" + heart;
            if(heart == 0)
            {
                Debug.Log("GameOver");
                GameObject.Find("LevelLoader").GetComponent<LevelLoader>().Reload();
            }
        }
    }

    [SerializeField] private Text coinLabel;
    private int coin;
    public int Coin
    {
        get { return coin; }
        set { 
            coin = value;
            coinLabel.GetComponent<Text>().text = "x" + coin;
        }
    }

    private void Awake()
    {
        m_screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }
    void Start()
    {
        m_isGateOpened = false;
        m_grumpy.SetSelected(true);
        m_playerPassed = 0;
        Coin = 0;
        Heart = 2;
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
        if (m_grumpy.IsSelected())
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

    public void OpenGate()
    {
        m_isGateOpened = true;
        //Camera zoom in

        GameObject.Find("CameraZoom").GetComponent<CameraZoom>().ZoomIn();

        StartCoroutine(TransformGateState(2f, CameraBehavior.ZoomIn));
        StartCoroutine(TransformGateState(2.5f, CameraBehavior.ZoomOut));

    }

    IEnumerator TransformGateState(float sec, CameraBehavior cbh)
    {
        if (cbh == CameraBehavior.ZoomIn)
        {
            yield return new WaitForSeconds(sec);
            //Change Gate's spriteRenderer
            GateScript gateObject = GameObject.Find("Gate").GetComponent<GateScript>();
            gateObject.OpenGate();
        }
        else if (cbh == CameraBehavior.ZoomOut)
        {
            yield return new WaitForSeconds(sec);
            GameObject.Find("CameraZoom").GetComponent<CameraZoom>().ZoomOut();
        }
    }

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
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel();
        //GameObject.Find("LevelLoader").GetComponent<LevelLoader>().Reload();
    }

    public void PlayerPassTheGate(GameObject go)
    {
        if (m_isGateOpened)
        {
            go.SetActive(false);
            m_playerPassed++;
        }
        if (m_playerPassed == 2)
        {
            WinGame();
        }
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
