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

    private readonly float _broadcastUpdateInterval = 0.5f;
    private float _currentUpdateInterval;
    private bool _hasConnected;

    // Lista temporária utilizada para ler as partidas encontradas pelo Network Discovery.
    private List<NetworkBroadcastResult> _matches = new List<NetworkBroadcastResult>();

    private void Start()
    {
        NetworkManagerSingleton.Discovery.Initialize();
    }

    public void SearchForMatches()
    {
        _searchMatchButton.SetActive(false);
        NetworkManagerSingleton.Discovery.StartAsClient();
    }

    public void CreateMatch()
    {
        NetworkManagerSingleton.Discovery.StopBroadcast();

        NetworkManagerSingleton.Discovery.broadcastData = _matchName.text;

        NetworkManagerSingleton.Discovery.StartAsServer();
        NetworkManagerSingleton.singleton.StartHost();

        _hasConnected = true;
    }

    
    private void Update()
    {
        if (!_hasConnected)
        {
            _currentUpdateInterval -= Time.deltaTime;
            if (_currentUpdateInterval < 0)
            {
                RefreshMatches();

                _currentUpdateInterval = _broadcastUpdateInterval;
            }
        }
    }

    private void RefreshMatches()
    {
        //_matches.Clear();

        foreach (var match in NetworkManagerSingleton.Discovery.broadcastsReceived.Values)
        {
            if(_matches.Any(item => EqualsArray(item.broadcastData, match.broadcastData)))
            {
                continue;
            }

            _matches.Add(match);

            string __matchName = Encoding.Unicode.GetString(match.broadcastData);

            GameObject __matchInstance = Instantiate(_matchPrefab, _matchPrefab.transform.position, _matchPrefab.transform.rotation);
            __matchInstance.transform.parent = _matchesList.transform;

            __matchInstance.GetComponentsInChildren<Text>()[0].text = __matchName;
        }

        // for(int i=0; i < _matches.Count; i++)
        // {
        //     string __matchName = Encoding.Unicode.GetString(_matches[i].broadcastData);

        //     GameObject __matchInstance = Instantiate(_matchPrefab, _matchPrefab.transform.position, _matchPrefab.transform.rotation);
        //     __matchInstance.transform.parent = _matchesList.transform;

        //     __matchInstance.GetComponentsInChildren<Text>()[0].text = __matchName;
        // }
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
