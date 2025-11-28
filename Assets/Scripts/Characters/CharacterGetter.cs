using AYellowpaper.SerializedCollections;
using UnityEngine;

public class CharacterGetter : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<string, Character> _characters;

    public Character GetCharacter(string name)
    {
        return _characters.ContainsKey(name)
            ? _characters[name]
            : null;
    }

    public Sprite GetCharacterSprite(string name, Character.CharacterDisplay display = Character.CharacterDisplay.Front)
    {
        return _characters.ContainsKey(name)
            ? _characters[name].GetSprite(display)
            : null;
    }
}
