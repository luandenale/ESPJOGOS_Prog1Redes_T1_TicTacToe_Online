using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManagers : MonoBehaviour
{
    [SerializeField] GameObject _gameManagerObject;
    [SerializeField] GameObject _networkGameManagerObject;

    void Update()
    {
        if(GameMode.mode == Mode.IA || GameMode.mode == Mode.SINGLE)
        {
            _gameManagerObject.SetActive(true);
            _networkGameManagerObject.SetActive(false);
        }
        else if (GameMode.mode == Mode.LAN || GameMode.mode == Mode.ONLINE)
        {
            _gameManagerObject.SetActive(false);
            _networkGameManagerObject.SetActive(true);
        }
    }
}
