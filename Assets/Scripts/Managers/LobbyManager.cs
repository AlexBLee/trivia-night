using System;
using System.Collections.Generic;
using Fleck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _teamText;
    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _gamePanel;

    [SerializeField] private Server _server;
    [SerializeField] private TeamManager _teamManager;

    private List<IWebSocketConnection> _connectedPlayers = new(new IWebSocketConnection[4]);

    private Queue<int> _connectingPlayers = new();
    private Queue<int> _disconnectingPlayers = new();

    private int _playerCount = 0;

    public void Start()
    {
        _server.OnConnected += OnConnected;
        _server.OnDisconnected += OnDisconnected;

        _startButton.onClick.AddListener(StartGame);
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

    private void OnConnected(IWebSocketConnection player)
    {
        for (int i = 0; i < _connectedPlayers.Count; i++)
        {
            if (_connectedPlayers[i] == null)
            {
                _connectedPlayers[i] = player;
                _connectingPlayers.Enqueue(i);
                return;
            }
        }
    }

    private void OnDisconnected(IWebSocketConnection player)
    {
        var index = _connectedPlayers.IndexOf(player);
        _connectedPlayers[index] = null;
        _disconnectingPlayers.Enqueue(index);
    }

    private void StartGame()
    {
        _teamManager.AssignTeams(_connectedPlayers);
        gameObject.SetActive(false);
        _gamePanel.SetActive(true);
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveAllListeners();
    }
}
