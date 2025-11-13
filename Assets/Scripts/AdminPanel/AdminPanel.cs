using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminPanel : MonoBehaviour
{
    [SerializeField] private TeamOptions _teamOptionsPrefab;
    [SerializeField] private GameObject _teamOptionsParent;

    [SerializeField] private UIManager _uiManager;

    [SerializeField] private Button _openFinalJeopardyButton;
    [SerializeField] private Button _openEndGameScreenButton;
    [SerializeField] private Button _addPlayersToLobbyButton;

    private List<TeamOptions> _teamOptions = new();

    private bool _showFinalJeopardy = false;
    private bool _showEndGameScreen = false;

    void Start()
    {
#if !UNITY_EDITOR
        Display.displays[1].Activate();
#endif

        _openFinalJeopardyButton.onClick.AddListener(() =>
        {
            _showFinalJeopardy = !_showFinalJeopardy;
            _uiManager.ShowFinalJeopardy(_showFinalJeopardy);
        });

        _openEndGameScreenButton.onClick.AddListener(() =>
        {
            _showEndGameScreen = !_showEndGameScreen;
            _uiManager.ShowEndGameScreen(_showEndGameScreen);
        });

        _addPlayersToLobbyButton.onClick.AddListener(() =>
        {
            _uiManager.AddLobbyLabel();
        });
    }

    public void AssignTeams(List<Team> team)
    {
        foreach (var t in team)
        {
            var teamOptions = Instantiate(_teamOptionsPrefab, _teamOptionsParent.transform);
            teamOptions.AssignTeam(t);
            
            _teamOptions.Add(teamOptions);
        }
    }

    private void OnDestroy()
    {
        _openFinalJeopardyButton.onClick.RemoveAllListeners();
        _openEndGameScreenButton.onClick.RemoveAllListeners();

        foreach (var teamOption in _teamOptions)
        {
            Destroy(teamOption.gameObject);
        }
        _teamOptions.Clear();
    }
}
