using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] GameObject _gameEndedLine;

    private SlotController[] _slotControllers;
    public SlotController[] SlotControllers { get { return _slotControllers; } }

    private string[,] _board = new string[3, 3];
    public string[,] Board { get { return _board; } }

    private Transform _gameEndedInitialTransform;
    private Animator _boardAnimator;

    private void Awake()
    {
        _slotControllers = GetComponentsInChildren<SlotController>();
        _gameEndedInitialTransform = _gameEndedLine.transform;
        _boardAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.instance.currentState != GameStates.RESTART)
            UpdateSlots();
    }

    void UpdateSlots()
    {
        for(int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                int __xPos = x;
                int __yPos = y;
                string __symbol = _board[x, y];

                if (__symbol != null)
                {
                    foreach (SlotController slot in _slotControllers)
                    {
                        if (slot.xPos == __xPos && slot.yPos == __yPos && !slot.selected)
                        {
                            slot.SetSymbolToSlot(__symbol);
                        }
                    }
                }
            }
        }
    }

    public void InitializeAnimation()
    {
        _boardAnimator.SetTrigger("InitialAnimation");
    }

    public Tuple<bool, string> CheckEndGame(string[,] p_board, bool p_mainCheck)
    {
        if (p_board == null)
            p_board = _board;
        int __xPos = -1;
        int __yPos = -1;
        int __diagonalLine = 0;

        bool __gameEnded = false;
        string __whoWon = "";

        int __totalSum = 0;

        // Check if board is full
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (p_board[x, y] != null) __totalSum++;
            }
        }
        if (__totalSum == 9)
            __gameEnded = true;

        // Check if line completed with 3 equal symbols
        for (int y = 0; y < 3; y++)
        {
            int __xSum = 0;
            int __oSum = 0;
            for (int x = 0; x < 3; x++)
            {
                if (p_board[x, y] == "o") __oSum++;
                if (p_board[x, y] == "x") __xSum++;
            }

            if (__oSum > 2 || __xSum > 2)
            {
                __xPos = y;
                if (__oSum > 2) __whoWon = "o";
                else __whoWon = "x";
            }
        }

        // Check if column completed with 3 equal symbols
        for (int x = 0; x < 3; x++)
        {
            int __xSum = 0;
            int __oSum = 0;
            for (int y = 0; y < 3; y++)
            {
                if (p_board[x, y] == "o") __oSum++;
                if (p_board[x, y] == "x") __xSum++;
            }

            if (__oSum > 2 || __xSum > 2)
            {
                __yPos = x;
                if (__oSum > 2) __whoWon = "o";
                else __whoWon = "x";
            }
        }

        // Check diagonals
        if (p_board[0, 0] == p_board[1, 1] && p_board[1, 1] == p_board[2, 2] && p_board[0, 0] != null)
        {
            __whoWon = p_board[0, 0];
            __diagonalLine = 1;
        }
        else if (p_board[0, 2] == p_board[1, 1] && p_board[1, 1] == p_board[2, 0] && p_board[0, 2] != null)
        {
            __whoWon = p_board[0, 2];
            __diagonalLine = 2;
        }

        if (__whoWon != "")
        {
            __gameEnded = true;

            // Set the Winner
            if (p_mainCheck)
            {
                if (__whoWon == "x")
                    GameManager.instance.currentState = GameStates.X_WON;
                else
                    GameManager.instance.currentState = GameStates.O_WON;

                SetWinningLine(__xPos, __yPos, __diagonalLine);
            }
        }
        else if(__gameEnded && p_mainCheck)
            GameManager.instance.currentState = GameStates.TIE;

        return Tuple.Create(__gameEnded, __whoWon);
    }

    public void SetWinningLine(int p_xPos, int p_yPos, int p_diagonalLine)
    {
        _gameEndedLine.SetActive(true);

        // Horizontal Line
        if (p_xPos > -1)
        {
            float __zTransformPos;

            if (p_xPos == 0) __zTransformPos = 3.4f;
            else if (p_xPos == 1) __zTransformPos = 0;
            else __zTransformPos = -3.4f;

            _gameEndedLine.transform.localEulerAngles = new Vector3(90f, 0f, 90f);
            _gameEndedLine.transform.localPosition = new Vector3(0f, 0.12f, __zTransformPos);
        }
        // Vertical Line
        else if (p_yPos > -1)
        {
            float __xTransformPos;

            if (p_yPos == 0) __xTransformPos = -3.4f;
            else if (p_yPos == 1) __xTransformPos = 0;
            else __xTransformPos = 3.4f;

            _gameEndedLine.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            _gameEndedLine.transform.localPosition = new Vector3(__xTransformPos, 0.12f, 0f);
        }
        // Diagonal Line
        else
        {
            if (p_diagonalLine == 1)
                _gameEndedLine.transform.localEulerAngles = new Vector3(90f, 0f, 45f);
            else
                _gameEndedLine.transform.localEulerAngles = new Vector3(90f, 0f, -45f);

            _gameEndedLine.transform.localPosition = new Vector3(0f, 0.12f, 0f);
        }

        _boardAnimator.SetBool("WinningLineAnimation", true);

    }


    public void ResetBoard()
    {
        _boardAnimator.SetBool("WinningLineAnimation", false);
        _board = new string[3, 3];

        _gameEndedLine.SetActive(false);
        _gameEndedLine.transform.position = _gameEndedInitialTransform.position;
        _gameEndedLine.transform.rotation = _gameEndedInitialTransform.rotation;
        _gameEndedLine.transform.localScale = _gameEndedInitialTransform.localScale;
    }
}
