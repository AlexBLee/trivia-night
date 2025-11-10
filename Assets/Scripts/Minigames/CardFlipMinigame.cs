using Fleck;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipMinigame : Minigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private GameObject _cardParent;

    private Card _shownCard;
    private Card _hiddenCard;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessageToServer("gambling");
        _finishButton.onClick.AddListener(FinishGame);

        SpawnCards();
    }

    private void SpawnCards()
    {
        if (_shownCard != null)
        {
            Destroy(_shownCard.gameObject);
        }

        if (_hiddenCard != null)
        {
            Destroy(_hiddenCard.gameObject);
        }

        _shownCard = Instantiate(_cardPrefab, _cardParent.transform);
        _shownCard.ShowFace();

        _hiddenCard = Instantiate(_cardPrefab, _cardParent.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnCards();
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
