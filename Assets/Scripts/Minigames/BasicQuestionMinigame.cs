using UnityEngine;
using UnityEngine.UI;

public class BasicQuestionMinigame : Minigame
{
    [SerializeField] private Button _finishButton;

    private void OnEnable()
    {
        _finishButton.onClick.AddListener(FinishGame);
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("Basic QuestionMinigame");
    }

    protected override void FinishGame()
    {
        base.FinishGame();
    }

    private void OnDisable()
    {
        _finishButton.onClick.RemoveListener(FinishGame);
    }
}
