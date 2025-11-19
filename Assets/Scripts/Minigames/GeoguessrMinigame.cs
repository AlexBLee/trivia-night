using System.Collections.Generic;
using System.Linq;
using Fleck;
using Mapbox.Unity.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class GeoguessrMinigame : Minigame
{
    [SerializeField] private GeoguessrMapView _geoguessrMapView;
    [SerializeField] private GeoguessrResult _geoguessrResultPrefab;
    [SerializeField] private GameObject _resultParent;

    [SerializeField] private GameObject _uiParent;
    [SerializeField] private Button _finishButton;
    [SerializeField] private Button _finishDisplayingMapButton;
    [SerializeField] private Image _image;
    [SerializeField] private Image _background;

    [SerializeField] private GameObject _mapContainer;
    [SerializeField] private GameObject _endResultContainer;

    [SerializeField] private Button _closeButton;

    private List<GeoguessrResult> _resultPrefabs = new List<GeoguessrResult>();

    private Dictionary<Team, string> _guesses = new();
    private Dictionary<Team, int> _results = new();

    private (double, double) _point;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessageToServer("geoguessr");

        _finishButton.onClick.AddListener(DisplayGuesses);
        _finishDisplayingMapButton.onClick.AddListener(FinishDisplayingMap);
        _closeButton.onClick.AddListener(FinishGame);

        _uiParent.gameObject.SetActive(true);
        _endResultContainer.gameObject.SetActive(false);
        _finishDisplayingMapButton.gameObject.SetActive(false);

        var image = Resources.Load<Sprite>(minigameData.Input);
        _image.sprite = image;

        var coords = minigameData.Answer.Split(',');
        _geoguessrMapView.Initialize(coords);
    }

    private void DisplayGuesses()
    {
        _mapContainer.SetActive(true);
        _finishDisplayingMapButton.gameObject.SetActive(true);
        _background.gameObject.SetActive(false);

        foreach (var guess in _guesses)
        {
            var score = _geoguessrMapView.GetScore(guess.Value);

            if (!_results.TryAdd(guess.Key, score))
            {
                _results[guess.Key] = score;
            }

            _geoguessrMapView.SpawnMarkerOnMap(Conversions.StringToLatLon(guess.Value), guess.Key.TeamName);
            guess.Key.AddScore(score);
        }

        _geoguessrMapView.FitCameraBounds();
        _uiParent.gameObject.SetActive(false);
    }

    protected override void ReceiveMessage(IWebSocketConnection socket, string message)
    {
        base.ReceiveMessage(socket, message);

        var team = _teamManager.GetTeam(socket);
        if (_guesses.ContainsKey(team))
        {
            _guesses[team] = message;
        }
        else
        {
            _guesses.Add(team, message);
        }
    }

    private void FinishDisplayingMap()
    {
        _background.gameObject.SetActive(true);
        _uiParent.gameObject.SetActive(true);
        _mapContainer.gameObject.SetActive(false);
        _endResultContainer.gameObject.SetActive(true);

        var resultsList = _results
            .OrderBy(score => score.Value)
            .Reverse()
            .ToList();

        for (int i = 0; i < resultsList.Count; i++)
        {
            var resultPrefab = Instantiate(_geoguessrResultPrefab, _resultParent.transform);
            var result = resultsList[i];

            resultPrefab.SetLabel(result.Key.TeamName, result.Value);

            _resultPrefabs.Add(resultPrefab);
        }
    }

    protected override void FinishGame()
    {
        base.FinishGame();

        foreach (var resultPrefab in _resultPrefabs)
        {
            Destroy(resultPrefab.gameObject);
        }
        _resultPrefabs.Clear();

        _finishButton.onClick.RemoveListener(DisplayGuesses);
        _finishDisplayingMapButton.onClick.RemoveListener(FinishDisplayingMap);
        _closeButton.onClick.RemoveListener(FinishGame);
    }
}
