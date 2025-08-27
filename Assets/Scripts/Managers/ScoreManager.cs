using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private List<Team> _teams = new List<Team>();

    public void GrantScore(Team team, int score)
    {
        team.AddScore(score);
    }

    public void RemoveScore(Team team, int score)
    {
        team.RemoveScore(score);
    }

    
}
