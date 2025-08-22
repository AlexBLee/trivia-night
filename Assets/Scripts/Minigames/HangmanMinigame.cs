using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangmanMinigame : Minigame
{
    [SerializeField] private string _word;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _finishButton;

    public override void Initialize()
    {
        base.Initialize();
        SendMessage("hangman");

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

    protected override void ReceiveMessage(string message)
    {
        base.ReceiveMessage(message);
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _finishButton.onClick.RemoveListener(FinishGame);
    }
}
