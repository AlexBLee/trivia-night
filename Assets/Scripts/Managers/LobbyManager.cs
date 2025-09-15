using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private MessageManager _messageManager;
    [SerializeField] private TextMeshProUGUI _ipText;

    private List<IWebSocketConnection> _connectedPlayers = new(new IWebSocketConnection[4]);
    private List<string> _teamNames = new();

    private Queue<int> _connectingPlayers = new();
    private Queue<int> _disconnectingPlayers = new();
    private Queue<(int, string)> _namingTeamQueue = new();

    private int _playerCount = 0;

    public void Start()
    {
        _server.OnConnected += OnConnected;
        _server.OnDisconnected += OnDisconnected;

        _messageManager.OnMessageReceived += OnMessageReceived;
        _startButton.onClick.AddListener(StartGame);

        _ipText.text = $"{ServerExtensions.GetLocalIpv4Address()}:8081";
    }

    private void OnMessageReceived(IWebSocketConnection socket, string teamName)
    {
        if (_connectedPlayers.Contains(socket))
        {
            int index = _connectedPlayers.IndexOf(socket);
            if (index > -1)
            {
                teamName = teamName.Split(':')[1];
                _namingTeamQueue.Enqueue((index, teamName));
                _teamNames.Add(teamName);
            }
        }
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

        if (_namingTeamQueue.Count > 0)
        {
            var team = _namingTeamQueue.Dequeue();
            _teamText[team.Item1].text = team.Item2;
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
        _messageManager.SendMessageToServer("home");
        _teamManager.AssignTeams(_connectedPlayers, _teamNames);
        gameObject.SetActive(false);
        _gamePanel.SetActive(true);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveAllListeners();
        _server.OnConnected -= OnConnected;
        _server.OnDisconnected -= OnDisconnected;
        _messageManager.OnMessageReceived -= OnMessageReceived;
    }
}
