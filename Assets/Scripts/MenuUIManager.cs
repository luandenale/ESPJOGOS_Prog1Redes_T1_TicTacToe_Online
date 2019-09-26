﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField]
    private Text _popUpText;
    private Animator _menuAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _menuAnimator = GetComponent<Animator>();
    }

    public void PlayLocal()
    {
        _menuAnimator.SetTrigger("Play Local");
    }

    public void LocalBackToMenu()
    {
        _menuAnimator.SetTrigger("Back To Menu");
    }

    public void OnlineSelected()
    {
        _menuAnimator.SetTrigger("Play Online");
    }

    public void OnlineBackToMenu()
    {
        _menuAnimator.SetTrigger("Back To Play Online");
    }

    public void V1Start()
    {
        _menuAnimator.SetTrigger("1v1 Start");
        GameManager.instance.currentState = GameStates.STARTING;
    }

    public void VCpuSelected()
    {
        _menuAnimator.SetTrigger("1vCPU Selection");
    }

    public void DifficultyBackToLocal()
    {
        _menuAnimator.SetTrigger("Back To Play Local");
    }

    public void VCpuStart(int p_difficulty)
    {
        _menuAnimator.SetTrigger("1vCPU Start");
        GameManager.instance.Difficulty = p_difficulty;
        GameManager.instance.currentState = GameStates.STARTING;
    }

    public void LanSelect()
    {
        _menuAnimator.SetTrigger("Pop Up Lobby");
    }

    public void CreateMatch()
    {
        _menuAnimator.SetTrigger("Pop Up Canvas");
    }

    public void WaitingOponent()
    {
        _menuAnimator.SetTrigger("Waiting Oponent");
        _popUpText.text = "WAITING OPONENT...";
    }

    public void OponentConnected()
    {
        _menuAnimator.SetTrigger("Pop Up Canvas");
        _menuAnimator.SetTrigger("Oponent Connected");
        StartCoroutine(TextCountdown());
    }

    private IEnumerator TextCountdown()
    {
        _popUpText.text = "OPONENT CONNECTED\nSTARTING IN 3";
        yield return new WaitForSeconds(1f);
        _popUpText.text = "OPONENT CONNECTED\nSTARTING IN 2";
        yield return new WaitForSeconds(1f);
        _popUpText.text = "OPONENT CONNECTED\nSTARTING IN 1";
        yield return new WaitForSeconds(1f);
        _popUpText.text = "game started";
    }

    public void InternetSelect()
    {
        _menuAnimator.SetTrigger("Pop Up Lobby");
    }

    public void CloseLobby()
    {
        _menuAnimator.SetTrigger("Pop Down Lobby");
    }
}
