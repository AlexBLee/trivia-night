using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _teamText;
    [SerializeField] private Server _server;

    private List<string> _connectedPlayers = new(new string[4]);

    private Queue<int> _connectingPlayers = new();
    private Queue<int> _disconnectingPlayers = new();

    private int _playerCount = 0;

    public void Start()
    {
        _server.OnConnected += OnConnected;
        _server.OnDisconnected += OnDisconnected;
    }

    private void Update()
    {
        // Need to do this instead of using OnConnected because server stuff isnt on the main thread :(
        if (_connectingPlayers.Count > 0)
        {
            _teamText[_connectingPlayers.Dequeue()].color = Color.yellow;
        }

        if (_disconnectingPlayers.Count > 0)
        {
            _teamText[_disconnectingPlayers.Dequeue()].color = Color.white;
        }
    }

    private void OnConnected(string clientId)
    {
        for (int i = 0; i < _connectedPlayers.Count; i++)
        {
            if (string.IsNullOrEmpty(_connectedPlayers[i]))
            {
                _connectedPlayers[i] = clientId;
                _connectingPlayers.Enqueue(i);
                return;
            }
        }
    }

    private void OnDisconnected(string clientId)
    {
        var index = _connectedPlayers.IndexOf(clientId);
        _connectedPlayers[index] = string.Empty;
        _disconnectingPlayers.Enqueue(index);
    }
}
