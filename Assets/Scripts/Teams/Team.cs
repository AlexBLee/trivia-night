using System;

public class Team
{
    private string _teamName = "Team";
    private int _score;

    public string TeamName => _teamName;
    public int CurrentScore => _score;

    public Action<int> OnScoreChanged;

    public void AssignTeamName(string teamName)
    {
        _teamName = teamName;
    }

    public void AddScore(int score)
    {
        _score += score;
        OnScoreChanged?.Invoke(_score);
    }

    public void SetScore(int score)
    {
        _score = score;
        OnScoreChanged?.Invoke(_score);
    }

    public void RemoveScore(int score)
    {
        _score -= score;
        OnScoreChanged?.Invoke(_score);
    }
}
