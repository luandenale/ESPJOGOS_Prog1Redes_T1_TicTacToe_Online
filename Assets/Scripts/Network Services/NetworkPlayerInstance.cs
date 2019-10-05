using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerInstance : NetworkBehaviour 
{
    private string _lastPlayer = "o";
    private void Start()
    {
        if (isServer)
        {
            NetworkPlayerHandler.RegisterPlayer(this);
        }
    }

    [Command]
    private void CmdResetBoard()
    {
        NetworkPlayerHandler.ResetBoard();
    }

    // Chamado pelo servidor, executa no cliente
    [ClientRpc]
    private void RpcResetBoard()
    {
        // garante que só é chamado no player que é dono do objeto
        if (!isLocalPlayer) return;

        _lastPlayer = "o";
        NetworkGameManager.instance.ActivateRestart();
    }

    public void ResetBoard()
    {
        RpcResetBoard();
    }

    [Command]
    private void CmdToMenu()
    {
        NetworkPlayerHandler.ToMenu();
    }

    // Chamado pelo servidor, executa no cliente
    [ClientRpc]
    private void RpcToMenu()
    {
        // garante que só é chamado no player que é dono do objeto
        if (!isLocalPlayer) return;

        NetworkGameManager.instance.ReloadAllGame();
    }

    public void ToMenu()
    {
        RpcToMenu();
    }

    // Chamado pelo cliente, executa no servidor
    [Command]
    private void CmdDoMove(int p_xPos, int p_yPos, string p_playerSymbol)
    {
        NetworkPlayerHandler.UpdateValue(p_xPos, p_yPos, p_playerSymbol, true);
    }

    // Chamado pelo servidor, executa no cliente
    [ClientRpc]
    private void RpcUpdateValue(int p_xPos, int p_yPos, string p_playerSymbol)
    {
        // garante que só é chamado no player que é dono do objeto
        if (!isLocalPlayer) return;

        _lastPlayer = p_playerSymbol;
        NetworkPlayerHandler.UpdateValue(p_xPos, p_yPos, p_playerSymbol, false);
    }


    // Roda apenas no servidor. Replica atualização para os clientes.
    public void UpdateValue(int p_xPos, int p_yPos, string p_playerSymbol)
    {
        RpcUpdateValue(p_xPos, p_yPos, p_playerSymbol);
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if(_lastPlayer == "x")
        {
            if(isServer)
                NetworkGameManager.instance.currentPlay = "OPPONENTS TURN...";
            else
            {
                NetworkGameManager.instance.currentPlay = "YOUR TURN, PLACE THE 'O'";
                ClickSquare();
            }
        }
        else
        {
            if(isServer)
            {
                NetworkGameManager.instance.currentPlay = "YOUR TURN, PLACE THE 'X'";
                ClickSquare();
            }
            else
                NetworkGameManager.instance.currentPlay = "OPPONENTS TURN...";
        }

        // Should restart everybody
        if(NetworkGameManager.instance.setToMenu)
            CmdToMenu();
        else if(NetworkGameManager.instance.setToRestart)
            CmdResetBoard();

    }

    private void ClickSquare()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //TODO: ADD SOUND MANAGER LATER
                //_audioManager.PlayClickSlot();
                SlotController __slot = hit.transform.gameObject.GetComponent<SlotController>();

                string __playerSymbol;
                if(isServer)
                    __playerSymbol = "x";
                else
                    __playerSymbol = "o";

                // Avisa servidor qual posição cliquei e que símbolo sou
                CmdDoMove(__slot.xPos, __slot.yPos, __playerSymbol);
            }
        }
    } 
}
