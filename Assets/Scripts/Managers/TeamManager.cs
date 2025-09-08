using System.Collections.Generic;
using Fleck;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    private Dictionary<IWebSocketConnection, Team> _teams = new();

    public void AssignTeams(List<IWebSocketConnection> connections)
    {
        foreach (var connection in connections)
        {
            if (connection == null)
            {
                continue;
            }

            var team = new Team();
            _teams.Add(connection, team);
        }
    }

    public Team GetTeam(IWebSocketConnection connection)
    {
        return _teams[connection];
    }

    public void GrantScore(IWebSocketConnection team, int score)
    {
        _teams[team].AddScore(score);
    }

    public void RemoveScore(IWebSocketConnection team, int score)
    {
        _teams[team].RemoveScore(score);
    }
}
