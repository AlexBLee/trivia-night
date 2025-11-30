using Fleck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PictureGuessMinigame : Minigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private Image _image;
    [SerializeField] private Image _resultImage;
    [SerializeField] private TextMeshProUGUI _resultText;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessageToServer("pictureGuess");
        _finishButton.onClick.AddListener(FinishGame);

        _image.sprite = Resources.Load<Sprite>(minigameData.Input);
        _resultImage.sprite = Resources.Load<Sprite>(minigameData.Answer);
        // _resultText.text = minigameData.Answer;

        _uiManager.ShowCharacters();

        // _resultText.gameObject.SetActive(false);
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
        // _resultText.gameObject.SetActive(true);
    }

    protected override void ReceiveMessage(IWebSocketConnection socket, string message)
    {
        base.ReceiveMessage(socket, message);
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _uiManager.ShowCharacters(false);
        _finishButton.onClick.RemoveListener(FinishGame);
    }
}