using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TeamLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _teamNameText;
    [SerializeField] private TextMeshProUGUI _teamScoreText;

    private float _updateInterval = 0.05f;
    private float _timer;
    private bool _shouldDisplay = true;

    private int _teamScore;

    public void AssignTeamAttributes(Team team)
    {
        _teamNameText.text = team.TeamName;
        _teamScore = team.CurrentScore;
    }

    void Update()
    {
        if (!_shouldDisplay) return;

        _timer += Time.deltaTime;
        if (_timer >= _updateInterval)
        {
            _teamScoreText.text = Random.Range(1000, 9999).ToString();
            _timer = 0f;
        }
    }

    public void StopDisplay()
    {
        _shouldDisplay = false;
        _teamScoreText.text = _teamScore.ToString();
    }
}
