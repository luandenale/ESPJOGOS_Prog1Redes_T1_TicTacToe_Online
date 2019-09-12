using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILibrary : MonoBehaviour
{

    // Método principal para chamar Minimax
    public void PlaysMinimax(BoardController p_boardController)
    {
        int __xPos = -1;
        int __yPos = -1;

        int __rankValue = -1;
        int __deep = 999999;

        // Aplicando Minimax em cada slot vazio
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (p_boardController.Board[x, y] == null)
                {
                    string[,] __newBoard = (string[,])p_boardController.Board.Clone();
                    __newBoard[x, y] = "o";

                    Tuple<int, int> __miniMaxResult = MiniMax(__newBoard, true, 0, p_boardController);

                    int __newRank = __miniMaxResult.Item1;
                    int __newDeep = __miniMaxResult.Item2;

                    if (GameManager.instance.useDeep)
                    {
                        // Se foi achada uma jogada melhor, ou ao menos igual
                        if (__newRank > __rankValue || (__newRank == __rankValue && __newDeep <= __deep))
                        {
                            if(__newDeep == __deep)
                            {
                                // Escolher randomicamente se deve atualizar rank
                                if(UnityEngine.Random.Range(0f,100f) < 50f)
                                {
                                    __rankValue = __newRank;
                                    __deep = __newDeep;
                                    __xPos = x;
                                    __yPos = y;
                                }
                            }
                            else // newDeep < deep
                            {
                                __rankValue = __newRank;
                                __deep = __newDeep;
                                __xPos = x;
                                __yPos = y;
                            }
                        }
                    }
                    else
                    {
                        // Se foi achada uma jogada melhor, ou ao menos igual
                        if (__newRank >= __rankValue)
                        {
                            if(__newRank == __rankValue)
                            {
                                // Escolher randomicamente se deve atualizar rank
                                if(UnityEngine.Random.Range(0f,100f) < 50f)
                                {
                                    __rankValue = __newRank;
                                    __xPos = x;
                                    __yPos = y;
                                }
                            }
                            else // newDeep < deep
                            {
                                __rankValue = __newRank;
                                __xPos = x;
                                __yPos = y;
                            }
                        }
                    }
                }
            }
        }

        // Encontrou alguma jogada
        if (__xPos > -1)
        {
            p_boardController.Board[__xPos, __yPos] = "o";
        }
    }

    // Método recursivo do Minimax que retorna o rank e a profundidade
    Tuple<int, int> MiniMax(string[,] p_board, bool p_playerTurn, int p_deep, BoardController p_boardController)
    {
        int __deep = p_deep + 1;

        Tuple<bool, string> __checkEndGameReturn = p_boardController.CheckEndGame(p_board, false);
        bool __gameEnded = __checkEndGameReturn.Item1;
        string __whoWon = __checkEndGameReturn.Item2;

        if (__gameEnded)
        {
            return Tuple.Create(CalcScore(__whoWon), __deep);
        }

        int __val;

        // Simula jogada do jogador
        if (p_playerTurn)
        {
            __val = 9999999;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (p_board[x, y] == null)
                    {
                        string[,] __newBoard = (string[,])p_board.Clone();
                        __newBoard[x, y] = "x";

                        Tuple<int, int> __miniMaxResult = MiniMax(__newBoard, false, __deep, p_boardController);

                        __val = Math.Min(__val, __miniMaxResult.Item1);
                        __deep = __miniMaxResult.Item2;
                    }
                }
            }
        }
        // Simula jogada da IA
        else
        {
            __val = -9999999;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (p_board[x, y] == null)
                    {
                        string[,] __newBoard = (string[,])p_board.Clone();
                        __newBoard[x, y] = "o";

                        Tuple<int, int> __miniMaxResult = MiniMax(__newBoard, true, __deep, p_boardController);

                        __val = Math.Max(__val, __miniMaxResult.Item1);
                        __deep = __miniMaxResult.Item2;
                    }
                }
            }
        }

        return Tuple.Create(__val, __deep);
    }

    int CalcScore(string p_whoWon)
    {
        if (p_whoWon == "x")
            return -1;
        if (p_whoWon == "o")
            return 1;

        return 0;
    }
}
