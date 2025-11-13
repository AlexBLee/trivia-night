using System.Collections.Generic;
using Fleck;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private LobbyView _lobbyView;

    [SerializeField] private Server _server;
    [SerializeField] private TeamManager _teamManager;
    [SerializeField] private MessageManager _messageManager;

    private List<IWebSocketConnection> _connectedPlayers = new(new IWebSocketConnection[4]);
    private List<string> _teamNames = new();

    private Queue<int> _disconnectingPlayers = new();
    private Queue<(int, string)> _namingTeamQueue = new();

    private int _playerCount = 0;

    public void Start()
    {
        _server.OnConnected += OnConnected;
        _server.OnDisconnected += OnDisconnected;

        _lobbyView.AddStartGameCallback(StartGame);
        _messageManager.OnMessageReceived += OnMessageReceived;
    }

    private void OnConnected(IWebSocketConnection player)
    {
        for (int i = 0; i < _connectedPlayers.Count; i++)
        {
            if (_connectedPlayers[i] == null)
            {
                _connectedPlayers[i] = player;
                return;
            }
        }

        _connectedPlayers.Add(player);
    }

    private void OnDisconnected(IWebSocketConnection player)
    {
        var index = _connectedPlayers.IndexOf(player);
        _connectedPlayers[index] = null;
        _disconnectingPlayers.Enqueue(index);
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
        if (_disconnectingPlayers.Count > 0)
        {
            var player = _disconnectingPlayers.Dequeue();
            _lobbyView.ChangeLabel(player, "Team", Color.white);
        }

        if (_namingTeamQueue.Count > 0)
        {
            var team = _namingTeamQueue.Dequeue();
            _lobbyView.ChangeLabel(team.Item1, team.Item2, Color.yellow);
        }
    }

    private void StartGame()
    {
        _messageManager.SendMessageToServer("home");
        _teamManager.AssignTeams(_connectedPlayers, _teamNames);
    }

    private void OnDisable()
    {
        _server.OnConnected -= OnConnected;
        _server.OnDisconnected -= OnDisconnected;
        _messageManager.OnMessageReceived -= OnMessageReceived;
    }
}
