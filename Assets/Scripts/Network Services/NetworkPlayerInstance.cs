﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerInstance : NetworkBehaviour 
{
    private void Start()
    {
        if (isServer)
        {
            NetworkPlayerHandler.RegisterPlayer(this);
        }
    }

    // Chamado pelo cliente, executa no servidor
    [Command]
    private void CmdDoMove(int p_xPos, int p_yPos, string p_playerSymbol)
    {
        NetworkPlayerHandler.UpdateValue(p_xPos, p_xPos, p_playerSymbol, true);
    }

    // Chamado pelo servidor, executa no cliente
    [ClientRpc]
    private void RpcUpdateValue(int p_xPos, int p_yPos, string p_playerSymbol)
    {
        // garante que só é chamado no player que é dono do objeto
        if (!isLocalPlayer) return;

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

        ClickSquare();
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