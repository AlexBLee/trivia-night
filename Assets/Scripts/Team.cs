using Fleck;

public class Team
{
    private int _score;

    public int CurrentScore => _score;

    public void AddScore(int score)
    {
        _score += score;
    }

    public void RemoveScore(int score)
    {
        _score -= score;
    }
}
