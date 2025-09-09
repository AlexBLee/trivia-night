using System;
using System.Collections.Generic;
using Fleck;
using UnityEngine;
using UnityEngine.UI;

public class GeoguessrMinigame : Minigame
{
    [SerializeField] private Button _finishButton;
    [SerializeField] private Image _image;

    private Dictionary<Team, string> _guesses = new();
    private Dictionary<Team, int> _results = new();

    private (double, double) _point;

    private int _maxPoints = 500;
    private float _maxDistance = 10;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        SendMessage("geoguessr");
        _finishButton.onClick.AddListener(FinishGame);

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

    [ContextMenu("Finish Game")]
    private void DisplayGuesses()
    {
        foreach (var guess in _guesses)
        {
            var score = GetScoreFromDistance(GetDistanceInKm(guess.Value));

            if (!_results.TryAdd(guess.Key, score))
            {
                _results[guess.Key] = score;
            }

            Debug.Log(score);
        }
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

    protected override void FinishGame()
    {
        base.FinishGame();
        _finishButton.onClick.RemoveListener(FinishGame);
    }
}
