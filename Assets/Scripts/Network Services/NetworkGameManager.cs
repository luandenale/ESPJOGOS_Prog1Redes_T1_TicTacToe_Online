using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkGameManager : MonoBehaviour
{
    [SerializeField] Animator _inGameButtonsAnimator;
    [SerializeField] FallingPiecesManager _fallingPieces;
    [SerializeField] Animator _fadeAnimator;
    [SerializeField] Text _currentPlayerText;

    private bool _gameEnded = false;

    public  AudioManager audioManager;
    public BoardController boardController;
    public static NetworkGameManager instance = null;
    public GameStates currentState = GameStates.MENU;
    public int activatedSlots = 0;

    public bool setToRestart = false;
    public bool setToMenu = false;

    public string currentPlay = "";


    private void Start()
    {
        if(NetworkManagerSingleton.singleton!=null)
        {
            NetworkManagerSingleton.singleton.StopHost();
            NetworkManagerSingleton.singleton.StopClient();
        }
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameStates.STARTING:
                StartCoroutine(StartAnimation());
                break;
            case GameStates.RUNNING:
                _currentPlayerText.text = currentPlay;
                boardController.CheckEndGame(null, true);
                break;
            case GameStates.O_WON:
                if (!_gameEnded)
                {
                    _gameEnded = true;
                    _currentPlayerText.text = "'O' PLAYER WON!";
                
                    _fallingPieces.FallPieces("o");
                    audioManager.PlayWinYay();
                }
                break;
            case GameStates.X_WON:
                if (!_gameEnded)
                {
                    _gameEnded = true;
                    _currentPlayerText.text = "'X' PLAYER WON!";
                    _fallingPieces.FallPieces("x");
                    audioManager.PlayWinYay();
                }
                break;
            case GameStates.TIE:
                _currentPlayerText.text = "GAME TIED";
                 break;
            case GameStates.RESTART:
                ResetGame();
                break;
            default:
                break;
        }

        // Checking if board was reseted (no activated slots)
        if (activatedSlots <= 0 && currentState == GameStates.RESTART)
        {
            activatedSlots = 0;
            currentState = GameStates.RUNNING;
        }
    }

    

    public void ActivateRestart()
    {
        setToRestart = false;
        StartCoroutine(RestartAnimation());
    }

    IEnumerator RestartAnimation()
    {
        _fadeAnimator.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        currentState = GameStates.RESTART;
        _fadeAnimator.Play("FadeIn");
        yield return new WaitForSeconds(0.5f);
    }

    public void ReloadAllGame()
    {
        StartCoroutine(ReloadGameAnimation());
    }

    IEnumerator ReloadGameAnimation()
    {
        _fadeAnimator.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(1.2f);
        boardController.InitializeAnimation();
        yield return new WaitForSeconds(1.35f);

        _inGameButtonsAnimator.SetTrigger("SlideIn");

        currentState = GameStates.RUNNING;
    }

    private void ResetGame()
    {
        _gameEnded = false;
        currentPlay = "";
        _fallingPieces.ResetPieces();
        boardController.ResetBoard();
    }
}
