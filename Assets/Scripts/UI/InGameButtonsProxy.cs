using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButtonsProxy : MonoBehaviour
{
    public void ResetGame()
    {
        if(GameMode.mode == Mode.IA || GameMode.mode == Mode.SINGLE)
            GameManager.instance.ActivateRestart();
        else
            NetworkGameManager.instance.setToRestart = true;
    }

    public void ToMenu()
    {
        if(GameMode.mode == Mode.IA || GameMode.mode == Mode.SINGLE)
            GameManager.instance.ReloadAllGame();
        else
            NetworkGameManager.instance.ReloadAllGame();
    }
}
