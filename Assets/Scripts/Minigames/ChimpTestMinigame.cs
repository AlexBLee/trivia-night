using System;
using System.Collections.Generic;
using Fleck;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChimpTestGame : Minigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private RectTransform _gridParent;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private int _gridRows = 5;
    [SerializeField] private int _gridCols = 5;

    private List<Tile> _activeTiles = new();
    private List<RectTransform> _cellSlots = new List<RectTransform>();
    private int _startCount;
    private int _currentCount;
    private int _nextExpectedNumber;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessageToServer("chimpTest");

        int.TryParse(minigameData.Input, out _startCount);

        StartGame();
        _finishButton.onClick.AddListener(FinishGame);
    }

    protected override void ReceiveMessage(IWebSocketConnection socket, string message)
    {
        base.ReceiveMessage(socket, message);
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        ClearTiles();
        _finishButton.onClick.RemoveListener(FinishGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HideTiles();
        }
    }

    private void StartGame()
    {
        _nextExpectedNumber = 1;
        _currentCount = _startCount;

        SetupGrid();
        SpawnTiles(_currentCount);
    }

    private void HideTiles()
    {
        foreach (var tile in _activeTiles)
            tile.HideNumber();
    }

    private void SetupGrid()
    {
        ClearTiles();

        int totalCells = _gridRows * _gridCols;
        for (int i = 0; i < totalCells; i++)
        {
            var placeholder = new GameObject("Cell", typeof(RectTransform));
            var rect = placeholder.GetComponent<RectTransform>();
            rect.SetParent(_gridParent, false);
            _cellSlots.Add(rect);
        }
    }

    private void SpawnTiles(int count)
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < _cellSlots.Count; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = 1; i <= count; i++)
        {
            int rand = Random.Range(0, availableIndices.Count);
            int cellIndex = availableIndices[rand];
            availableIndices.RemoveAt(rand);

            var tile = Instantiate(_tilePrefab, _cellSlots[cellIndex]);
            tile.Init(i, OnTileClicked);
            _activeTiles.Add(tile);
        }
    }

    private void OnTileClicked(Tile tile)
    {
        if (tile.Number == _nextExpectedNumber)
        {
            tile.gameObject.SetActive(false);

            _nextExpectedNumber++;

            if (_nextExpectedNumber > _currentCount)
            {
                _currentCount++;
            }
        }
        else
        {
            _currentCount = _startCount;
            StartGame();
        }
    }

    private void ClearTiles()
    {
        foreach (Transform child in _gridParent)
            Destroy(child.gameObject);

        _cellSlots.Clear();
        _activeTiles.Clear();
    }
}
