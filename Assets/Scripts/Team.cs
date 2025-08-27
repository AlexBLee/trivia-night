using Fleck;

public class Team
{
    private IWebSocketConnection _socketConnection;
    private int _score;

    public int CurrentScore => _score;

    public void AssignSocketConnection(IWebSocketConnection socketConnection)
    {
        _socketConnection = socketConnection;
    }

    public void AddScore(int score)
    {
        _score += score;
    }

    public void RemoveScore(int score)
    {
        _score -= score;
    }
}
