using TMPro;
using UnityEngine;

public class TeamAnswerLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _teamName;
    [SerializeField] private TextMeshProUGUI _score;

    public void SetLabel(string teamName, int score)
    {
        _teamName.text = teamName;
        _score.text = score.ToString();
    }
}
