using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminPanel : MonoBehaviour
{
    [SerializeField] private List<TeamOptions> _teamOptions;

    [SerializeField] private UIManager _uiManager;

    [SerializeField] private Button _openFinalJeopardyButton;
    [SerializeField] private Button _openEndGameScreenButton;
    [SerializeField] private Button _addPlayersToLobbyButton;

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
        for (int i = 0; i < team.Count; i++)
        {
            _teamOptions[i].AssignTeam(team[i]);
        }
    }

    private void OnDestroy()
    {
        _openFinalJeopardyButton.onClick.RemoveAllListeners();
        _openEndGameScreenButton.onClick.RemoveAllListeners();
    }
}
