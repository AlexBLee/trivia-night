using System.Collections;
using Fleck;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CoinFlipMinigame : Minigame
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Button _finishButton;
    [SerializeField] private float _flipTime = 5f;

    private const string FlipAnimation = "Flipping";
    private const string HeadsAnimation = "Heads";
    private const string TailsAnimation = "Tails";

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessageToServer("gambling");

        _finishButton.onClick.AddListener(FinishGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(StartCoinFlip());
        }
    }

    private IEnumerator StartCoinFlip()
    {
        if (_animator == null)
        {
            yield return null;
        }

        _animator.Play(FlipAnimation);

        yield return new WaitForSeconds(_flipTime);

        var maxRandomValue = 2;
        var decider = Random.Range(0, maxRandomValue);

        _animator.Play(decider == 0 ? HeadsAnimation : TailsAnimation);
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
