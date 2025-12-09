using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LobbyView _lobbyView;
    [SerializeField] private TeamManager _teamManager;

    [SerializeField] private GameObject _gameSelectionContainer;
    [SerializeField] private GameObject _endGameContainer;
    [SerializeField] private GameObject[] _finalJeopardyButtons;
    [SerializeField] private MultiCharacterDisplay _multiCharacterDisplay;

    public void ShowGameSelection(bool show)
    {
        _gameSelectionContainer.gameObject.SetActive(show);
    }

    public void AddLobbyLabel()
    {
        _lobbyView.CreateTeamLabels(1);
    }

    public void ShowCharacters(bool show = true)
    {
        if (show)
        {
            _multiCharacterDisplay.DisplayCharacters(_teamManager.Teams.Values.ToList(), Character.CharacterDisplay.Back);
            _multiCharacterDisplay.gameObject.SetActive(true);
        }
        else
        {
            _multiCharacterDisplay.HideCharacters();
        }
    }

    public void AnimateCharacter(Team team)
    {
        _multiCharacterDisplay.AnimateCharacterJump(team);
    }

    public void ShowFinalJeopardy(bool show)
    {
        foreach (var button in _finalJeopardyButtons)
        {
            button.SetActive(show);
        }
    }

    public void ShowEndGameScreen(bool show)
    {
        _gameSelectionContainer.gameObject.SetActive(!show);
        _endGameContainer.gameObject.SetActive(show);
    }
}
