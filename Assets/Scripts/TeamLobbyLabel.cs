using TMPro;
using UnityEngine;

public class TeamLobbyLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void ChangeLabel(string nameText, Color color)
    {
        _text.color = color;
        _text.text = nameText;
    }
}
