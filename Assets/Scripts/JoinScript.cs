using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinScript : MonoBehaviour
{
    [SerializeField]
    GameObject m_charMenu;

    public void Connect()
    {
        string hostID = GameObject.Find("Menus/JoinMenu/InputPlayerID").GetComponent<TMP_InputField>().text;
        if (string.IsNullOrEmpty(hostID))
            return;
        Participant client = (Participant)ConnectionController.CreateParticipantClient(hostID);
        client.m_OnOpen += JoinRequest;
        client.m_OnAcceptJoin += OnAcceptJoin;
    }

    // Update is called once per frame
    void Update()
    {
        ConnectionController.Update();
    }

    public void OnBack()
    {
        ConnectionController.Close();
    }

    void JoinRequest()
    {
        ((Participant)ConnectionController.GetClient()).JoinRequest();
        ConnectionController.GetClient().m_OnOpen -= JoinRequest;
    }

    void OnAcceptJoin()
    {
        gameObject.SetActive(false);
        m_charMenu.SetActive(true);
    }
}
