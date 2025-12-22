using System;
using Fleck;
using UnityEngine;

public class Team
{
    private IWebSocketConnection _socket;
    private string _teamName = "Team";
    private string _characterName = "";
    private int _score;

    public string TeamName => _teamName;
    public string CharacterName => _characterName;
    public int CurrentScore => _score;

    public IWebSocketConnection Socket => _socket;

    public Action<int> OnScoreChanged;
    public Action<bool> OnConnectionStatusChanged;

    // TODO: REMOVE LATER, TEMPORARY MEASURE FOR SAFETY
    public void AssignSocket(IWebSocketConnection socket)
    {
        _socket = socket;
    }

    public void AssignTeamName(string teamName)
    {
        _teamName = teamName;
    }

    public void AssignCharacterName(string characterName)
    {
        _characterName = characterName;
    }

    public void AddScore(int score)
    {
        Debug.Log($"{TeamName} - Score Added: {_score} -> {_score + score}");
        _score += score;
        OnScoreChanged?.Invoke(_score);
    }

    public void SetScore(int score)
    {
        Debug.Log($"{TeamName} - Score Set: {_score} -> {score}");
        _score = score;
        OnScoreChanged?.Invoke(_score);
    }

    public void RemoveScore(int score)
    {
        _score -= score;
        OnScoreChanged?.Invoke(_score);
    }

    public void SetConnectionStatus(bool status)
    {
        OnConnectionStatusChanged?.Invoke(status);
    }
}
