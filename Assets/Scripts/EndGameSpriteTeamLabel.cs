using TMPro;
using UnityEngine;

public class SpriteTeamLabel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _teamNameText;
    [SerializeField] private TextMeshPro _scoreText;

    public void Initialize(Sprite sprite, string teamName, string score)
    {
        _spriteRenderer.sprite = sprite;
        _teamNameText.text = teamName;
        _scoreText.text = score;
    }
}
