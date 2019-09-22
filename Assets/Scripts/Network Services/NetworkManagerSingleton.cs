using UnityEngine.Networking;
using System;
using UnityEngine;

public class NetworkManagerSingleton : NetworkManager
{
    public static event Action<NetworkConnection> onServerConnect;

    public static NetworkDiscovery Discovery
    {
        get
        {
            return singleton.GetComponent<NetworkDiscovery>();
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
}
