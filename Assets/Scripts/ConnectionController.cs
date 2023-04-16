using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conn = TCPConnection;

public class ConnectionController
{
    static Client m_client;
    static string m_playerID;

    public static Client GetClient()
    {
        return m_client;
    }

    public static Client CreateHostClient()
    {
        if (m_client != null) m_client = null;
        if (string.IsNullOrEmpty(m_playerID))
            m_playerID = GetLocalIPAddress();
        m_client = new Host(m_playerID, "0.0.0.0");
        return m_client;
    }

    public static Conn CreateParticipantClient(string hostID)
    {
        if (m_client != null) m_client = null;
        if (string.IsNullOrEmpty(m_playerID))
            m_playerID = GetLocalIPAddress();
        m_client = new Participant(m_playerID, hostID);
        return m_client;
    }

    public static string GetPlayerID()
    {
        return m_playerID;
    }

    public static void Close()
    {
        if (m_client != null)
        {
            m_client.Close();
            m_client = null;
        }
    }

    public static void Update()
    {
        if (m_client != null)
        {
            m_client.Update();
        }
    }

    public static bool ConnectionIsOpen()
    {
        return m_client != null && m_client.IsOpen();
    }

    private static string GenShortID()
    {
        var base64Guid = System.Convert.ToBase64String(System.Guid.NewGuid().ToByteArray());
        return base64Guid.Substring(0, base64Guid.Length - 2);
    }

    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        Debug.Log(host.AddressList);
        foreach (var ip in host.AddressList)
        {
            Debug.Log(ip);
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
}
