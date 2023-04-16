using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HostScript : MonoBehaviour
{
    [SerializeField]
    GameObject m_charMenu;

    // Start is called before the first frame update
    void Start()
    {
        //var conn = ConnectionController.OpenConnection();
        //GameObject.Find("Menus/HostMenu/InputPlayerID").GetComponent<TMP_InputField>().text = conn.GetPlayerID();
    }

    private void OnEnable()
    {
        var client = (Host)ConnectionController.CreateHostClient();
        GameObject.Find("Menus/HostMenu/InputPlayerID").GetComponent<TMP_InputField>().text = client.GetPlayerID();
        client.m_OnJoinRequest += OnJoinRequest;
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

    private void OnJoinRequest(Message<JoinRequest> message)
    {
        Modal.GetInstance().ShowModal("Player with ID: " + message.username + " want to join your game?", "Accept", "Decline", AcceptJoin, () => {});
    }

    private void AcceptJoin()
    {
        var client = (Host)ConnectionController.GetClient();
        client.AcceptJoin();
        //client.m_OnJoinRequest -= OnJoinRequest;
        gameObject.SetActive(false);
        m_charMenu.SetActive(true);
    }

    public void CopyClientID()
    {
        TextEditor editor = new TextEditor
        {
            text = ConnectionController.GetClient().GetPlayerID()
        };
        editor.SelectAll();
        editor.Copy();
        Modal.GetInstance().ShowModal("ID copied.", "OK", () => { });
    }
}
