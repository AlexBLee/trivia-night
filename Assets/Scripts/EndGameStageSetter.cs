using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EndGameStageSetter : MonoBehaviour
{
    [SerializeField] private CharacterGetter _characterGetter;

    [SerializeField] private GameObject _placementPrefab;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _placementGap;

    [SerializeField] private Vector3 _spriteSpawnPositionOffset;
    [SerializeField] private Vector3 _spritePlacementOffset;
    [SerializeField] private SpriteTeamLabel _spriteTeamLabel;
    [SerializeField] private float _timeToFall;

    private List<SpriteTeamLabel> _spriteTeamLabels = new();
    private List<Tween> _tweens = new();

    private int _currentIndex = 0;

    public void AddPlacement(int index, Team team)
    {
        GameObject placement = Instantiate(_placementPrefab, transform);
        placement.transform.localPosition = _startPosition + _placementGap * index;

        var character = _characterGetter.GetCharacter(team.CharacterName);
        if (character == null)
        {
            return;
        }

        var placementLocalPosition = placement.transform.localPosition;

        var characterSprite = character.GetSprite(Character.CharacterDisplay.Front);
        SpriteTeamLabel spriteTeamLabel = Instantiate(_spriteTeamLabel, transform);

        _spriteTeamLabels.Add(spriteTeamLabel);
        _tweens.Add(spriteTeamLabel.transform.DOLocalMove(placementLocalPosition + _spritePlacementOffset, _timeToFall).SetAutoKill(false).Pause());

        spriteTeamLabel.gameObject.transform.localPosition = placementLocalPosition + _spriteSpawnPositionOffset;
        spriteTeamLabel.Initialize(characterSprite, team.TeamName, team.CurrentScore.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_tweens.Count <= 0 || _currentIndex >= _tweens.Count)
            {
                return;
            }

            _tweens[_currentIndex].Play();
            _currentIndex++;
        }
    }
}
