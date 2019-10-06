using UnityEngine.Networking;
using System;
using UnityEngine;
using UnityEngine.Networking.Match;

public class NetworkManagerSingleton : NetworkManager
{
    public static event Action<NetworkConnection> onServerConnect;
    public static event Action<NetworkConnection> onClientConnect;

    public static NetworkDiscovery Discovery
    {
        get
        {
            return singleton.GetComponent<NetworkDiscovery>();
        }
    }

    public static NetworkMatch Match
    {
        get
        {
            return singleton.GetComponent<NetworkMatch>() ?? singleton.gameObject.AddComponent<NetworkMatch>();
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if(conn.address == "localClient")
        {
            return;
        }

        Debug.Log("Client connected! Address: " + conn.address);

        if(onServerConnect != null)
        {
            onServerConnect(conn);
        }
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        if (conn.address == "localServer")
        {
            return;
        }

        Debug.Log("Connected to server! Address: " + conn.address);

        onClientConnect?.Invoke(conn);
    }
}
