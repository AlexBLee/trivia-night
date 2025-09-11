using System;
using System.Collections;
using System.Collections.Generic;
using Fleck;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GeoguessrMinigame : Minigame
{
    [SerializeField] private GameObject _uiParent;
    [SerializeField] private Button _finishButton;
    [SerializeField] private Image _image;

    [SerializeField] private AbstractMap _map;
    [SerializeField] private Camera _mapCamera;
    [SerializeField] private SpawnMarkersOnMap _spawnMarkersOnMap;

    private Dictionary<Team, string> _guesses = new();
    private Dictionary<Team, int> _results = new();

    private (double, double) _point;

    private int _maxPoints = 500;
    private float _maxDistance = 10;

    private float _zoomAmount = 16f;
    private float _zoomDuration = 2f;
    private float _timeBeforeZoom = 3f;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessage("geoguessr");
        _finishButton.onClick.AddListener(DisplayGuesses);
        _uiParent.gameObject.SetActive(true);

        var image = Resources.Load<Sprite>(minigameData.Input);
        _image.sprite = image;
        
        var coords = minigameData.Answer.Split(',');
        var latitude = Convert.ToDouble(coords[0]);
        var longitude = Convert.ToDouble(coords[1]);

        _point = (latitude, longitude);
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

    private void DisplayGuesses()
    {
        foreach (var guess in _guesses)
        {
            var score = GetScoreFromDistance(GetDistanceInKm(guess.Value));

            if (!_results.TryAdd(guess.Key, score))
            {
                _results[guess.Key] = score;
            }

            _spawnMarkersOnMap.SetMarker(guess.Value);
        }

        _uiParent.gameObject.SetActive(false);
        _map.gameObject.SetActive(true);
        _mapCamera.gameObject.SetActive(true);

        Vector2d coordinatePoint = new Vector2d(_point.Item1, _point.Item2);
        _map.SetCenterLatitudeLongitude(coordinatePoint);
        _spawnMarkersOnMap.SetMarker(coordinatePoint);

        StartCoroutine(ZoomCoroutine(_zoomAmount, _zoomDuration));
    }

    private double GetDistanceInKm(string answer)
    {
        double lat1 = _point.Item1;

        var coords = answer.Split(',');
        double lat2 = Convert.ToDouble(coords[0]);
        double lon2 = Convert.ToDouble(coords[1]);
        
        double earthRadius = 6371.0;

        double diffLat = (lat2 - _point.Item1) * Mathf.Deg2Rad;
        double diffLon = (lon2 - _point.Item2) * Mathf.Deg2Rad;

        lat1 *= Mathf.Deg2Rad;
        lat2 *= Mathf.Deg2Rad;

        double a = Mathf.Sin((float)(diffLat / 2)) * Mathf.Sin((float)(diffLat / 2)) +
                   Mathf.Cos((float)lat1) * Mathf.Cos((float)lat2) *
                   Mathf.Sin((float)(diffLon / 2)) * Mathf.Sin((float)(diffLon / 2));

        double angularDistance = 2 * Mathf.Asin(Mathf.Sqrt((float)a));

        return earthRadius * angularDistance;
    }

    private int GetScoreFromDistance(double km)
    {
        if (km < 0.1f) return _maxPoints;
        if (km > _maxDistance) return 0;
        return Mathf.Max(0, (int)(_maxPoints * (1 - (km / _maxDistance))));
    }

    private IEnumerator ZoomCoroutine(float targetZoom, float duration)
    {
        float startZoom = _map.AbsoluteZoom;
        Vector2d coord = new Vector2d(_point.Item1, _point.Item2);
        float elapsed = 0f;

        yield return new WaitForSeconds(_timeBeforeZoom);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float easedT = Mathf.SmoothStep(0f, 1f, t);

            float currentZoom = Mathf.Lerp(startZoom, targetZoom, easedT);
            _map.UpdateMap(coord, currentZoom);

            yield return null;
        }

        _map.UpdateMap(coord, targetZoom);
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _finishButton.onClick.RemoveListener(DisplayGuesses);
    }
}
