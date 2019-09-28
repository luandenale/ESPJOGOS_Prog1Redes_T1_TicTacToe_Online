using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum NetworkGameStates
{
    MENU,
    STARTING,
    RUNNING,
    RESTART,
    X_WON,
    O_WON,
    TIE
}

public class NetworkGameManager : MonoBehaviour
{
    //[SerializeField] GameObject _currentPlayerUIGameObject;
    [SerializeField] BoardController _boardController;
    //[SerializeField] Animator _inGameButtonsAnimator;
    [SerializeField] FallingPiecesManager _fallingPieces;
    [SerializeField] Animator _fadeAnimator;

    //private AudioManager _audioManager;
    // private Text _currentPlayerText;
    //private Animator _currentPlayerAnimator;
    private bool _notPlayed = true;
    private bool _startsPlaying = true;
    private bool _gameEnded = false;
    
    private string _currentPlayer = "x";
    public string CurrentPlayer
    {
        get
        {
            return _currentPlayer;
        }
    }

    public static NetworkGameManager instance = null;
    public NetworkGameStates currentState = NetworkGameStates.MENU;
    public int activatedSlots = 0;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // _currentPlayerText = _currentPlayerUIGameObject.GetComponent<Text>();
        // _currentPlayerAnimator = _currentPlayerUIGameObject.GetComponent<Animator>();
        // _audioManager = GetComponentInChildren<AudioManager>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case NetworkGameStates.STARTING:
                StartCoroutine(StartAnimation());
                break;
            case NetworkGameStates.RUNNING:
                // _currentPlayerText.text = "PLAYER '" + _currentPlayer.ToUpper() + "' TURN...";
                // if (_cpuTurn && _cpuPlaying && _notPlayed)
                //     StartCoroutine(CPUPlay());
                // else if(!_cpuTurn)
                //     ClickSquare();
                _boardController.CheckEndGame(null, true);
                break;
            case NetworkGameStates.O_WON:
                if (!_gameEnded)
                {
                    _gameEnded = true;
                    // _currentPlayerAnimator.SetBool("Glow", false);
                    // _currentPlayerText.text = "'O' PLAYER WON!";
                
                    _fallingPieces.FallPieces("o");
                    // _audioManager.PlayWinYay();
                }
                break;
            case NetworkGameStates.X_WON:
                if (!_gameEnded)
                {
                    _gameEnded = true;
                    // _currentPlayerAnimator.SetBool("Glow", false);
                    // _currentPlayerText.text = "'X' PLAYER WON!";
                    _fallingPieces.FallPieces("x");
                    // _audioManager.PlayWinYay();
                }
                break;
            case NetworkGameStates.TIE:
                //  _currentPlayerAnimator.SetBool("Glow", false);
                //  _currentPlayerText.text = "GAME TIED";
                 break;
            // case NetworkGameStates.RESTART:
            //     ResetGame();
            //     break;
            default:
                break;
        }

        // Checking if board was reseted (no activated slots)
        // if (activatedSlots <= 0 && currentState == GameStates.RESTART)
        // {
        //     _currentPlayerAnimator.SetBool("Glow", true);
        //     activatedSlots = 0;
        //     currentState = GameStates.RUNNING;
        // }
    }

    // Called only from UI
    // public void ToggleCPUStart()
    // {
    //     _cpuTurn = !_cpuTurn;

    //     _startsPlaying = !_cpuTurn;

    //     if (_cpuTurn)
    //         _currentPlayer = "o";
    //     else
    //         _currentPlayer = "x";
    // }

    // Called only from UI
    // public void ActivateRestart()
    // {
    //     StartCoroutine(RestartAnimation());
    // }

    // IEnumerator RestartAnimation()
    // {
    //     _fadeAnimator.Play("FadeOut");
    //     yield return new WaitForSeconds(0.5f);
    //     currentState = NetworkGameStates.RESTART;
    //     _fadeAnimator.Play("FadeIn");
    //     yield return new WaitForSeconds(0.5f);
    // }

    // Called only from UI
    // public void ReloadAllGame()
    // {
    //     StartCoroutine(ReloadGameAnimation());
    // }

    // IEnumerator ReloadGameAnimation()
    // {
    //     _fadeAnimator.Play("FadeOut");
    //     yield return new WaitForSeconds(0.5f);
    //     Destroy(gameObject);
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    // }

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(1.2f);
        _boardController.InitializeAnimation();
        yield return new WaitForSeconds(1.35f);

        // _currentPlayerAnimator.SetTrigger("Initiate");
        // _inGameButtonsAnimator.SetTrigger("SlideIn");

        currentState = NetworkGameStates.RUNNING;
    }
    
    void SwitchPlayer()
    {
        if(_currentPlayer == "x")
            _currentPlayer = "o";
        else
            _currentPlayer = "x";
    }

    void ResetGame()
    {
        _notPlayed = true;
        _gameEnded = false;
        if (_startsPlaying)
        {
            _currentPlayer = "x";
        }
        else
        {
            _currentPlayer = "o";
        }
        _fallingPieces.ResetPieces();
        _boardController.ResetBoard();
    }
}
