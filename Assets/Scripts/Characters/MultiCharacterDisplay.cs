using System.Collections.Generic;
using UnityEngine;

public class MultiCharacterDisplay : MonoBehaviour
{
    [SerializeField] private CharacterGetter _characterGetter;
    [SerializeField] private TeamDisplay _teamDisplayPrefab;
    [SerializeField] private Transform _characterDisplayContainer;

    private List<TeamDisplay> _teamDisplayList = new List<TeamDisplay>();

    public void DisplayCharacters(List<Team> teams, Character.CharacterDisplay characterDisplay = Character.CharacterDisplay.Front)
    {
        if (_teamDisplayList.Count >= teams.Count)
        {
            return;
        }

        Clean();

        foreach (var team in teams)
        {
            var teamDisplay = Instantiate(_teamDisplayPrefab, _characterDisplayContainer);
            var character = _characterGetter.GetCharacterSprite(team.CharacterName, characterDisplay);

            teamDisplay.ChangeLabel(team.TeamName, Color.white);
            teamDisplay.SetImage(character);

            _teamDisplayList.Add(teamDisplay);
        }
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
