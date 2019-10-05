using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkGameManager : MonoBehaviour
{
    //[SerializeField] GameObject _currentPlayerUIGameObject;
    [SerializeField] Animator _inGameButtonsAnimator;
    [SerializeField] FallingPiecesManager _fallingPieces;
    [SerializeField] Animator _fadeAnimator;

    //private AudioManager _audioManager;
    [SerializeField] Text _currentPlayerText;
    //private Animator _currentPlayerAnimator;
    private bool _gameEnded = false;
    public BoardController boardController;
    public static NetworkGameManager instance = null;
    public GameStates currentState = GameStates.MENU;
    public int activatedSlots = 0;

    public bool setToRestart = false;

    public string currentPlay = "";


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
                    // _currentPlayerAnimator.SetBool("Glow", false);
                    _currentPlayerText.text = "'O' PLAYER WON!";
                
                    _fallingPieces.FallPieces("o");
                    // _audioManager.PlayWinYay();
                }
                break;
            case GameStates.X_WON:
                if (!_gameEnded)
                {
                    _gameEnded = true;
                    // _currentPlayerAnimator.SetBool("Glow", false);
                    _currentPlayerText.text = "'X' PLAYER WON!";
                    _fallingPieces.FallPieces("x");
                    // _audioManager.PlayWinYay();
                }
                break;
            case GameStates.TIE:
                //  _currentPlayerAnimator.SetBool("Glow", false);
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

    // Called only from UI
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

        // _currentPlayerAnimator.SetTrigger("Initiate");
        _inGameButtonsAnimator.SetTrigger("SlideIn");

        currentState = GameStates.RUNNING;
    }

    private void ResetGame()
    {
        Debug.Log("hello");
        _gameEnded = false;
        currentPlay = "";
        _fallingPieces.ResetPieces();
        boardController.ResetBoard();
    }
}
