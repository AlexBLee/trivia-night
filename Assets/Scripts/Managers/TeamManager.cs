using System.Collections.Generic;
using Fleck;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private AdminPanel _adminPanel;

    private Dictionary<IWebSocketConnection, Team> _teams = new();

    public void AssignTeams(List<IWebSocketConnection> connections)
    {
        List<Team> teams = new();

        foreach (var connection in connections)
        {
            if (connection == null)
            {
                continue;
            }

            var team = new Team();
            _teams.Add(connection, team);
            teams.Add(team);
        }

        _adminPanel.AssignTeams(teams);
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
