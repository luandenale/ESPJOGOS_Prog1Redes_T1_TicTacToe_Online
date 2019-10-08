using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkConnectionProxy : MonoBehaviour
{
    [SerializeField] LANMatchManager _lanMatchManager;
    [SerializeField] OnlineMatchManager _onlineMatchManager;
    public void CreateMatch()
    {
        if(GameMode.mode == Mode.LAN)
            _lanMatchManager.CreateMatch();
        else
            _onlineMatchManager.CreateMatch();
    }

    public void SearchMatch()
    {
        if(GameMode.mode == Mode.LAN)
            _lanMatchManager.SearchForMatches();
        else
            _onlineMatchManager.SearchForMatches();
    }
}
