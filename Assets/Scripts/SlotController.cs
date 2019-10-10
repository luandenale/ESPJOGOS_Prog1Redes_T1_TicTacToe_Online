using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    [SerializeField] GameObject _xSymbol;
    [SerializeField] GameObject _oSymbol;
    public int xPos;
    public int yPos;
    public bool selected = false;
    private bool _reseted = false;
    private Animator _pieceAnimator;

    private void Awake()
    {
        _pieceAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(GameMode.mode == Mode.IA || GameMode.mode == Mode.SINGLE)
        {
            if (GameManager.instance.currentState == GameStates.RESTART)
            {
                if (!_reseted)
                {
                    GetComponent<BoxCollider>().enabled = true;
                    _pieceAnimator.Play("Empty Animation");

                    GameManager.instance.activatedSlots--;
                    _reseted = true;
                    selected = false;
                }
            }
            else
                _reseted = false;
        }
        else
        {
            if (NetworkGameManager.instance.currentState == GameStates.RESTART)
            {
                if (!_reseted)
                {
                    GetComponent<BoxCollider>().enabled = true;
                    _pieceAnimator.Play("Empty Animation");

                    NetworkGameManager.instance.activatedSlots--;
                    _reseted = true;
                    selected = false;
                }
            }
            else
                _reseted = false;
        }
    }

    public void SetSymbolToSlot(string p_symbol)
    {
        selected = true;
        if(GameMode.mode == Mode.IA || GameMode.mode == Mode.SINGLE)
            GameManager.instance.activatedSlots++;
        else
            NetworkGameManager.instance.activatedSlots++;
        GetComponent<BoxCollider>().enabled = false;

        if (p_symbol == "x")
            _pieceAnimator.SetTrigger("ActivateX");
        else
            _pieceAnimator.SetTrigger("ActivateO");
    }
}
