using System.Collections.Generic;
using System.Linq;
using Fleck;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private AdminPanel _adminPanel;

    private Dictionary<IWebSocketConnection, Team> _teams = new();

    public Dictionary<IWebSocketConnection, Team> Teams => _teams;

    public void AssignTeams(List<IWebSocketConnection> connections, List<string> teamNames)
    {
        List<Team> teams = new();

        for (int i = 0; i < connections.Count; i++)
        {
            var connection = connections[i];

            if (connection == null)
            {
                continue;
            }

            var team = new Team();
            team.AssignTeamName(teamNames[i]);

            _teams.Add(connection, team);
            teams.Add(team);
        }

        _adminPanel.AssignTeams(teams);
    }

    public bool TrySearchForExistingTeamByIp(IWebSocketConnection connection)
    {
        return _teams.Any(team => team.Key.ConnectionInfo.ClientIpAddress == connection.ConnectionInfo.ClientIpAddress);
    }

    public void ReassignTeam(IWebSocketConnection connection)
    {
        IWebSocketConnection connectionToRemove = null;
        Team teamToAssign = null;

        foreach (var team in _teams)
        {
            if (team.Key.ConnectionInfo.ClientIpAddress == connection.ConnectionInfo.ClientIpAddress)
            {
                connectionToRemove = team.Key;
                teamToAssign = team.Value;
            }
        }

        if (connectionToRemove != null && teamToAssign != null)
        {
            _teams.Remove(connectionToRemove);
            _teams.Add(connection, teamToAssign);

            teamToAssign.SetConnectionStatus(true);
            Debug.Log("Reconnecting Team: " + connection.ConnectionInfo.ClientIpAddress + " - " + teamToAssign.TeamName);
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
