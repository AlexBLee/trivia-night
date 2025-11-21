using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = 1)]
public class Character : ScriptableObject
{
    public enum CharacterDisplay
    {
        Front,
        Back,
        Answer
    }

    [SerializeField] private string _characterName;
    [SerializeField] private Sprite _frontSprite;
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private Sprite _answerSprite;

    public Sprite GetSprite(CharacterDisplay display)
    {
        return display switch
        {
            CharacterDisplay.Front => _frontSprite,
            CharacterDisplay.Back => _backSprite,
            CharacterDisplay.Answer => _answerSprite,
            _ => throw new ArgumentOutOfRangeException(nameof(display), display, null)
        };
    }
}
