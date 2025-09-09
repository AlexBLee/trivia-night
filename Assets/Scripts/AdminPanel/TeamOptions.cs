using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamOptions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _teamNameText;
    [SerializeField] private TextMeshProUGUI _teamScoreText;

    [SerializeField] private Button _addHundredButton;
    [SerializeField] private Button _removeHundredButton;

    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _addScoreButton;
    [SerializeField] private Button _setScoreButton;

    private Team _team;

    public void AssignTeam(Team team)
    {
        _team = team;
        _teamNameText.text = team.TeamName;

        _team.OnScoreChanged += OnScoreChanged;

        _addHundredButton.onClick.AddListener(() => _team.AddScore(100));
        _removeHundredButton.onClick.AddListener(() => _team.RemoveScore(100));

        _addScoreButton.onClick.AddListener(() => _team.AddScore(Convert.ToInt32(_inputField.text)));
        _setScoreButton.onClick.AddListener(() => _team.SetScore(Convert.ToInt32(_inputField.text)));
    }

    private void OnScoreChanged(int score)
    {
        _teamScoreText.text = score.ToString();
    }

    private void OnDestroy()
    {
        if (_team != null)
        {
            _team.OnScoreChanged -= OnScoreChanged;
        }

        _addHundredButton.onClick.RemoveAllListeners();
        _removeHundredButton.onClick.RemoveAllListeners();
        _addScoreButton.onClick.RemoveAllListeners();
        _setScoreButton.onClick.RemoveAllListeners();
    }
}
