using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamLobbyLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _characterImage;

    public void ChangeLabel(string nameText, Color color)
    {
        _text.color = color;
        _text.text = nameText;
    }

    public void SetImage(Sprite sprite)
    {
        if (sprite == null)
        {
            _characterImage.gameObject.SetActive(false);
            return;
        }

        _characterImage.gameObject.SetActive(true);
        _characterImage.sprite = sprite;
    }
}
