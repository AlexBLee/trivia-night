using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

    private async void OnDisconnected(IWebSocketConnection player)
    {
        var index = _connectedPlayers.IndexOf(player);
        _connectedPlayers[index] = null;

        await UniTask.SwitchToMainThread();
        _lobbyView.ChangeTeamDisplay(index, "Team", Color.white);
    }

    private async void OnMessageReceived(IWebSocketConnection socket, string message)
    {
        if (_connectedPlayers.Contains(socket))
        {
            int index = _connectedPlayers.IndexOf(socket);
            if (index > -1)
            {
                var splitMessage = message.Split(':');
                var teamName = splitMessage[0];
                var characterName = splitMessage[1];

                _teamNames.Add(teamName);

                await UniTask.SwitchToMainThread();
                _lobbyView.ChangeTeamDisplay(index, teamName, Color.yellow, characterName);
            }
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
