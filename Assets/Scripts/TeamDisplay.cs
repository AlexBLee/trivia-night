using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _characterImage;

    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _jumpDuration = 0.5f;

    private Color _darkenedColour = new Color(0.5f, 0.5f, 0.5f, 1f);

    private Character _character;
    private Team _team;

    public Team Team => _team;

    public void ChangeLabel(string nameText, Color color)
    {
        _text.color = color;
        _text.text = nameText;
    }

    public void SetCharacter(Character character, Character.CharacterDisplay display = Character.CharacterDisplay.Front)
    {
        if (character == null)
        {
            _characterImage.gameObject.SetActive(false);
            return;
        }

        _character = character;

        _characterImage.gameObject.SetActive(true);
        _characterImage.sprite = character.GetSprite(display);
    }

    public void SetTeam(Team team)
    {
        _team = team;
    }

    public void AnimateCharacterAnswer()
    {
        _characterImage.sprite = _character.GetSprite(Character.CharacterDisplay.Answer);

        transform.DOLocalJump(transform.localPosition, _jumpHeight, 1, _jumpDuration)
            .OnComplete(() =>
            {
                _characterImage.sprite = _character.GetSprite(Character.CharacterDisplay.Back);
            });
    }

    public void DarkenCharacter()
    {
        _characterImage.color = _darkenedColour;
    }

    public void BrightenCharacter()
    {
        _characterImage.color = new Color(1f,1f,1f, 1f);
    }
}
