using System.Collections.Generic;
using Fleck;
using UnityEngine;
using UnityEngine.UI;

public class BombOrBonusMinigame : Minigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private PresentOrBombObject _presentPrefab;
    [SerializeField] private GameObject _horizontalLayoutGroup;
    [SerializeField] private int _numberOfPresents;
    [SerializeField] private int _numberOfBombs;

    private List<PresentOrBombObject> _spawnedPresents = new();

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessageToServer("gambling");

        var data = minigameData.Input.Split(",");
        _numberOfPresents = int.Parse(data[0]);
        _numberOfBombs = int.Parse(data[1]);

        _finishButton.onClick.AddListener(FinishGame);
        CreatePresents();
    }

    private void CreatePresents()
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < _numberOfPresents; i++)
        {
            indices.Add(i);
        }

        for (int i = 0; i < indices.Count; i++)
        {
            int randomIndex = Random.Range(i, indices.Count);
            (indices[i], indices[randomIndex]) = (indices[randomIndex], indices[i]);
        }

        HashSet<int> bombIndices = new HashSet<int>(indices.GetRange(0, _numberOfBombs));

        for (int i = 0; i < _numberOfPresents; i++)
        {
            var present = Instantiate(_presentPrefab, _horizontalLayoutGroup.transform);

            if (bombIndices.Contains(i))
            {
                present.SetBomb();
            }

            _spawnedPresents.Add(present);
        }
    }

    protected override void ReceiveMessage(IWebSocketConnection socket, string message)
    {
        base.ReceiveMessage(socket, message);
    }

    protected override void FinishGame()
    {
        base.FinishGame();

        foreach (var present in _spawnedPresents)
        {
            Destroy(present.gameObject);
        }

        _spawnedPresents.Clear();

        _finishButton.onClick.RemoveListener(FinishGame);
    }
}
