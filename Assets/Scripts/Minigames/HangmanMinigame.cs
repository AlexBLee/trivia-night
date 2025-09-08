using Fleck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangmanMinigame : Minigame
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _finishButton;

    private string _word;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessage("hangman");

        _word = minigameData.Input;
        string hiddenText = "";

        foreach (var ch in _word)
        {
            if (ch == ' ')
            {
                hiddenText += " ";
            }
            else
            {
                hiddenText += "-";
            }
        }

        _text.text = hiddenText;

        _finishButton.onClick.AddListener(FinishGame);
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(Input.inputString))
        {
            return;
        }

        foreach (char c in Input.inputString)
        {
            char[] chars = _text.text.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (_word.ToLower()[i] == c)
                {
                    chars[i] = c;
                }
            }

            _text.text = new string(chars);
        }
    }

    protected override void ReceiveMessage(IWebSocketConnection socket, string message)
    {
        base.ReceiveMessage(socket, message);
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _finishButton.onClick.RemoveListener(FinishGame);
    }
}
