using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
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
}
