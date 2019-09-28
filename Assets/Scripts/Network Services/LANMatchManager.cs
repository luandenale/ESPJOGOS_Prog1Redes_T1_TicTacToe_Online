using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Linq;
using System.Text;

public class LANMatchManager : MonoBehaviour
{
    [SerializeField]
    private InputField _matchName;
    [SerializeField]
    private GameObject _matchesList;
    [SerializeField]
    private GameObject _matchPrefab;
    [SerializeField]
    private GameObject _searchMatchButton;
    [SerializeField]
    private MenuUIManager _menuUIManager;

    private readonly float _broadcastUpdateInterval = 1f;
    private float _currentUpdateInterval;
    private bool _hasConnected;
    private bool _searching = false;

    private List<GameObject> _instantiatedGameObjects = new List<GameObject>();

    // Lista temporária utilizada para ler as partidas encontradas pelo Network Discovery.
    private List<NetworkBroadcastResult> _matches = new List<NetworkBroadcastResult>();

    private void Start()
    {
        NetworkManagerSingleton.Discovery.Initialize();
    }

    public void SearchForMatches()
    {
        _matches.Clear();
        _searchMatchButton.SetActive(false);
        NetworkManagerSingleton.Discovery.StartAsClient();
        _searching = true;
    }

    public void CreateMatch()
    {
        NetworkManagerSingleton.Discovery.StopBroadcast();

        NetworkManagerSingleton.Discovery.broadcastData = _matchName.text;

        NetworkManagerSingleton.Discovery.StartAsServer();
        NetworkManagerSingleton.singleton.StartHost();

        NetworkManagerSingleton.onServerConnect += OnServerConnect;

        _hasConnected = true;

        _menuUIManager.WaitingOponent();
    }

    
    private void Update()
    {
        if (!_hasConnected && _searching)
        {
            _currentUpdateInterval -= Time.deltaTime;
            if (_currentUpdateInterval < 0)
            {
                RefreshMatches();

                _currentUpdateInterval = _broadcastUpdateInterval;
            }
        }
    }

    private void OnMatchConnectClick(NetworkBroadcastResult p_match)
    {
        NetworkManagerSingleton.singleton.networkAddress = p_match.serverAddress;
        NetworkManagerSingleton.singleton.StartClient();

        NetworkManagerSingleton.Discovery.StopBroadcast();

        _hasConnected = true;

        _menuUIManager.OponentConnected();
    }

    private void OnServerConnect(NetworkConnection conn)
    {
        _menuUIManager.OponentConnected();
    }

    private void RefreshMatches()
    {
        foreach (GameObject __instance in _instantiatedGameObjects)
        {
            Destroy(__instance);
        }
        _matches.Clear();

        foreach (NetworkBroadcastResult __match in NetworkManagerSingleton.Discovery.broadcastsReceived.Values)
        {
            if (_matches.Any(__item => EqualsArray(__item.broadcastData, __match.broadcastData)))
            {
                continue;
            }

            string __matchName = Encoding.Unicode.GetString(__match.broadcastData);

            _matches.Add(__match);

            GameObject __matchInstance = Instantiate(_matchPrefab, _matchPrefab.transform.position, _matchPrefab.transform.rotation, _matchesList.transform);
            __matchInstance.GetComponentsInChildren<Text>()[0].text = __matchName;
            __matchInstance.GetComponentInChildren<Button>().onClick.AddListener((delegate 
            {
                OnMatchConnectClick(__match);
            }));

            _instantiatedGameObjects.Add(__matchInstance);
        }

        NetworkManagerSingleton.Discovery.broadcastsReceived.Clear();
    }

    private bool EqualsArray(byte[] left, byte[] right)
    {
        if (left.Length != right.Length)
        {
            return false;
        }
        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] != right[i])
            {
                return false;
            }
        }
        return true;
    }
}
