using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField]
    private Text _popUpText;
    private Animator _menuAnimator;

    [SerializeField] LANMatchManager _lanMatchManager;
    [SerializeField] OnlineMatchManager _onlineMatchManager;
    [SerializeField] GameObject _searchMatchButton;

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
        GameMode.mode = Mode.SINGLE;
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
        GameMode.mode = Mode.IA;
        _menuAnimator.SetTrigger("1vCPU Start");
        GameManager.instance.Difficulty = p_difficulty;
        GameManager.instance.currentState = GameStates.STARTING;
    }

    public void LanSelect()
    {
        GameMode.mode = Mode.LAN;
        _menuAnimator.SetTrigger("Pop Up Lobby");
    }

    public void CreateMatch()
    {
        _menuAnimator.SetTrigger("Pop Up Canvas");
    }

    public void WaitingOponent()
    {
        _menuAnimator.SetTrigger("Waiting Oponent");
        _popUpText.text = "WAITING OPPONENT...";
    }

    public void OponentConnected()
    {
        _menuAnimator.SetTrigger("Pop Up Canvas");
        _menuAnimator.SetTrigger("Oponnent Connected");
        StartCoroutine(TextCountdown());
    }

    private IEnumerator TextCountdown()
    {
        _popUpText.text = "OPPONENT CONNECTED\nSTARTING IN 3";
        yield return new WaitForSeconds(1f);
        _popUpText.text = "OPPONENT CONNECTED\nSTARTING IN 2";
        yield return new WaitForSeconds(1f);
        _popUpText.text = "OPPONENT CONNECTED\nSTARTING IN 1";
        yield return new WaitForSeconds(1f);
        _popUpText.text = "OPPONENT CONNECTED\nSTARTING NOW...";

        _menuAnimator.SetTrigger("Online Start");
        NetworkGameManager.instance.currentState = GameStates.STARTING;
    }

    public void InternetSelect()
    {
        GameMode.mode = Mode.ONLINE;
        _menuAnimator.SetTrigger("Pop Up Lobby");
    }

    public void CloseLobby()
    {
        _searchMatchButton.SetActive(true);
        if(GameMode.mode == Mode.LAN)
        {
            NetworkManagerSingleton.Discovery.StopBroadcast();
            _lanMatchManager.ClearMatches();
            _lanMatchManager.searching = false;
        }
        else
        {
            NetworkManagerSingleton.singleton.matches.Clear();
            _onlineMatchManager.ClearMatches();
            _onlineMatchManager.searching = false;       
        }
        _menuAnimator.SetTrigger("Pop Down Lobby");
    }
}
