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

    public void V1Start()
    {
        _menuAnimator.SetTrigger("1v1 Selected");
        GameManager.instance.currentState = GameStates.STARTING;
    }

    public void VCpuSelected()
    {
        _menuAnimator.SetBool("Back To Menu", false);
        _menuAnimator.SetBool("1vCpu Selected", true);
    }

    public void BackToMenu()
    {
        _menuAnimator.SetBool("Back To Menu", true);
        _menuAnimator.SetBool("1vCpu Selected", false);
    }

    public void VCpuStart(int p_difficulty)
    {
        _menuAnimator.SetTrigger("1vCpu Start");
        GameManager.instance.Difficulty = p_difficulty;
        GameManager.instance.currentState = GameStates.STARTING;
    }
}
