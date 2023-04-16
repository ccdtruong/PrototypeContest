using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PingScript : MonoBehaviour
{
    [SerializeField]
    GameObject btnSwap;

    // Start is called before the first frame update
    void Start()
    {
        if (GameSettings.m_mode != GameSettings.GameMode.Duo)
        {
            gameObject.SetActive(false);
            btnSwap.SetActive(true);
        } else
        {
            gameObject.SetActive(true);
            btnSwap.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSettings.m_mode == GameSettings.GameMode.Duo)
        {
            int ping = (int)ConnectionController.GetClient().GetPing();
            GetComponent<TextMeshProUGUI>().text = ping.ToString() + "ms";
        }
    }
}
