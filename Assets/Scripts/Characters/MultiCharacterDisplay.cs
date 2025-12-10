using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MultiCharacterDisplay : MonoBehaviour
{
    [SerializeField] private CharacterGetter _characterGetter;
    [SerializeField] private TeamDisplay _teamDisplayPrefab;
    [SerializeField] private Transform _characterDisplayContainer;

    [SerializeField] private float _characterDisplayShowHeight;
    [SerializeField] private float _characterDisplayHideHeight;

    private List<TeamDisplay> _teamDisplayList = new List<TeamDisplay>();

    public void DisplayCharacters(List<Team> teams, Character.CharacterDisplay characterDisplay = Character.CharacterDisplay.Front)
    {
        if (_teamDisplayList.Count > teams.Count)
        {
            return;
        }

        Clean();

        gameObject.transform.localPosition = new Vector3(0, _characterDisplayHideHeight, 0);

        foreach (var team in teams)
        {
            var teamDisplay = Instantiate(_teamDisplayPrefab, _characterDisplayContainer);
            var character = _characterGetter.GetCharacter(team.CharacterName);

            teamDisplay.ChangeLabel(team.TeamName, Color.white);
            teamDisplay.SetCharacter(character, characterDisplay);
            teamDisplay.SetTeam(team);

            _teamDisplayList.Add(teamDisplay);
        }

        gameObject.transform.DOLocalMoveY(_characterDisplayShowHeight, 1f);
    }

    public void AnimateCharacterJump(Team team)
    {
        var teamDisplay = _teamDisplayList.FirstOrDefault(t => t.Team == team);
        if (teamDisplay == null)
        {
            return;
        }

        teamDisplay.AnimateCharacterAnswer();
    }

    public void HideCharacters()
    {
        gameObject.transform.DOLocalMoveY(_characterDisplayHideHeight, 1f);
    }

    private void Clean()
    {
        foreach (var teamDisplay in _teamDisplayList)
        {
            Destroy(teamDisplay.gameObject);
        }

        _teamDisplayList.Clear();
    }

    private void OnDestroy()
    {
        Clean();
    }
}
