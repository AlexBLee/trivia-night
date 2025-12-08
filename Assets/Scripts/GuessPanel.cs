using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuessPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentTeamName;
    [SerializeField] private TextMeshProUGUI _numberOfPoints;
    [SerializeField] private Button _grantPointsButton;

    public void SetCurrentTeamAndPoints(Team team, int points)
    {
        _grantPointsButton.onClick.RemoveAllListeners();

        _currentTeamName.text = team.TeamName;
        _numberOfPoints.text = points.ToString();

        _grantPointsButton.onClick.AddListener(() => team.AddScore(points));
    }
}
