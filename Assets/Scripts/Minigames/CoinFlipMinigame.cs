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

        _animator.Play("Flipping");

        yield return new WaitForSeconds(_flipTime);

        var decider = Random.Range(0, 2);

        _animator.Play(decider == 0 ? "Heads" : "Tails");
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
