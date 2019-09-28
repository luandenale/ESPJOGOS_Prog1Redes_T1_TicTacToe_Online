using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkPlayerHandler
{

    [SerializeField] 
    private static BoardController _boardController;
    public static NetworkGameStates _currentState = NetworkGameStates.MENU;
    
    private static int[] _stateValues = new int[5];

    public static event Action OnStateUpdated;

    private static List<NetworkPlayerInstance> _players = new List<NetworkPlayerInstance>();

    // Roda apenas no servidor
    public static void RegisterPlayer(NetworkPlayerInstance player)
    {
        _players.Add(player);
    }

    public static void UpdateValue(int p_xPos, int p_yPos, string p_playerSymbol, bool isServer)
    {
        //Debug.Log($"UpdateValue called! pos:{pos} val:{val} isServer:{isServer}");

        // _stateValues[pos] = val;
        // OnStateUpdated?.Invoke();

        //Update my board
        //_boardController.CheckEndGame(null, true);
        _boardController.Board[p_xPos, p_yPos] = p_playerSymbol;

        //Avisa clientes para atualizarem seus boards
        if (isServer)
        {
            foreach (var player in _players)
            {
                player.UpdateValue(p_xPos, p_yPos, p_playerSymbol);
            }
        }
    }

    public static int GetValue(int pos)
    {
        return _stateValues[pos];
    }

    
}
