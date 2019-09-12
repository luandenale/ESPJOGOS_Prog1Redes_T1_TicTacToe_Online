using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameStates
{
    MENU,
    STARTING,
    RUNNING,
    RESTART,
    X_WON,
    O_WON,
    TIE
}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _currentPlayerUIGameObject;
    [SerializeField] BoardController _boardController;
    [SerializeField] Animator _inGameButtonsAnimator;
    [SerializeField] FallingPiecesManager _fallingPieces;
    [SerializeField] Animator _fadeAnimator;

    private AudioManager _audioManager;
    private Text _currentPlayerText;
    private Animator _currentPlayerAnimator;
    private AILibrary _ai;
    private bool _cpuPlaying = false;
    private bool _cpuTurn = false;
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

    private int _difficulty = 0;
    public int Difficulty
    {
        set
        {
            _difficulty = value;
        }
    }

    public static GameManager instance = null;
    public GameStates currentState = GameStates.MENU;
    public int activatedSlots = 0;
    public bool useDeep = false;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        _ai = GetComponent<AILibrary>();
        _currentPlayerText = _currentPlayerUIGameObject.GetComponent<Text>();
        _currentPlayerAnimator = _currentPlayerUIGameObject.GetComponent<Animator>();
        _audioManager = GetComponentInChildren<AudioManager>();
    }

    private void Update()
    {
        if (_difficulty > 0)
            _cpuPlaying = true;

        switch (currentState)
        {
            case GameStates.STARTING:
                StartCoroutine(StartAnimation());
                break;
            case GameStates.RUNNING:
                _currentPlayerText.text = "PLAYER '" + _currentPlayer.ToUpper() + "' TURN...";
                if (_cpuTurn && _cpuPlaying && _notPlayed)
                    StartCoroutine(CPUPlay());
                else if(!_cpuTurn)
                    ClickSquare();
                _boardController.CheckEndGame(null, true);
                break;
            case GameStates.O_WON:
                if (!_gameEnded)
                {
                    _gameEnded = true;
                    _currentPlayerAnimator.SetBool("Glow", false);
                    _currentPlayerText.text = "'O' PLAYER WON!";
                
                    _fallingPieces.FallPieces("o");
                    _audioManager.PlayWinYay();
                }
                break;
            case GameStates.X_WON:
                if (!_gameEnded)
                {
                    _gameEnded = true;
                    _currentPlayerAnimator.SetBool("Glow", false);
                    _currentPlayerText.text = "'X' PLAYER WON!";
                    _fallingPieces.FallPieces("x");
                    _audioManager.PlayWinYay();
                }
                break;
            case GameStates.TIE:
                _currentPlayerAnimator.SetBool("Glow", false);
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
            _currentPlayerAnimator.SetBool("Glow", true);
            activatedSlots = 0;
            currentState = GameStates.RUNNING;
        }
    }

    // Called only from UI
    public void ToggleCPUStart()
    {
        _cpuTurn = !_cpuTurn;

        _startsPlaying = !_cpuTurn;

        if (_cpuTurn)
            _currentPlayer = "o";
        else
            _currentPlayer = "x";
    }

    // Called only from UI
    public void ActivateRestart()
    {
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
        _boardController.InitializeAnimation();
        yield return new WaitForSeconds(1.35f);

        _currentPlayerAnimator.SetTrigger("Initiate");
        _inGameButtonsAnimator.SetTrigger("SlideIn");

        currentState = GameStates.RUNNING;
    }
    
    void ClickSquare()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _audioManager.PlayClickSlot();

                SlotController __slot = hit.transform.gameObject.GetComponent<SlotController>();

                _boardController.Board[__slot.xPos, __slot.yPos] = _currentPlayer;

                __slot.SetSymbolToSlot(_currentPlayer);

                SwitchPlayer();

                if (_cpuPlaying)
                {
                    _cpuTurn = true;
                    _notPlayed = true;
                }
            }
        }
    }

    IEnumerator CPUPlay()
    {
        _notPlayed = false;

        yield return new WaitForSeconds(0.5f);

        float __rand = UnityEngine.Random.Range(0f, 100f);

        switch (_difficulty)
        {
            case 1:
                PlaysRandom();
                break;
            case 2:
                if (__rand < 40f)
                    _ai.PlaysMinimax(_boardController);
                else
                    PlaysRandom();
                break;
            case 3:
                if (__rand < 80f)
                    _ai.PlaysMinimax(_boardController);
                else
                    PlaysRandom();
                break;
            case 4:
                _ai.PlaysMinimax(_boardController);
                break;
            default:
                break;
        }

        SwitchPlayer();

        _cpuTurn = false;
    }

    void PlaysRandom()
    {
        int __randomIndex = UnityEngine.Random.Range(0, _boardController.SlotControllers.Length);

        while (_boardController.SlotControllers[__randomIndex].selected)
            __randomIndex = UnityEngine.Random.Range(0, _boardController.SlotControllers.Length);

        SlotController __slot = _boardController.SlotControllers[__randomIndex];

        _boardController.Board[__slot.xPos, __slot.yPos] = _currentPlayer;

        __slot.SetSymbolToSlot(_currentPlayer);
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
            _cpuTurn = false;

            _currentPlayer = "x";
        }
        else
        {
            _cpuTurn = true;

            _currentPlayer = "o";
        }
        _fallingPieces.ResetPieces();
        _boardController.ResetBoard();
    }
}
