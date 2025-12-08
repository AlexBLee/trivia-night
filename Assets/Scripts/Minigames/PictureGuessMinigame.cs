using System;
using UnityEngine;
using UnityEngine.UI;

public class PictureGuessMinigame : SingleGuessMinigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private Image _image;
    [SerializeField] private Image _resultImage;
    public Action<Team, int> OnGuessClicked { get; set; }

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessageToServer("question");
        _finishButton.onClick.AddListener(FinishGame);

        _resultImage.gameObject.SetActive(false);

        var inputSprite = Resources.Load<Sprite>($"NotChristmas/{minigameData.Input}");
        _image.rectTransform.sizeDelta = inputSprite.rect.size;
        _image.sprite = inputSprite;

        var resultSprite = Resources.Load<Sprite>($"NotChristmas/{minigameData.Answer}");
        _resultImage.rectTransform.sizeDelta = resultSprite.rect.size;
        _resultImage.sprite = resultSprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReenableGuess();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            RevealAnswer();
        }
    }

    private void ReenableGuess()
    {
        SendMessageToServer("reenable");
    }

    private void RevealAnswer()
    {
        _resultImage.gameObject.SetActive(true);
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _finishButton.onClick.RemoveListener(FinishGame);
    }

}