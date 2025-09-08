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
    private (double, double) _point;

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

    private double GetDistanceInKm(string answer)
    {
        var coords = answer.Split(',');
        double lat2 = Convert.ToDouble(coords[1]);
        double lon2 = Convert.ToDouble(coords[2]);
        
        double earthRadius = 6371.0;

        double diffLat = (lat2 - _point.Item1) * Mathf.Deg2Rad;
        double diffLon = (lon2 - _point.Item2) * Mathf.Deg2Rad;

        _point.Item1 *= Mathf.Deg2Rad;
        lat2 *= Mathf.Deg2Rad;

        double a = Mathf.Sin((float)(diffLat / 2)) * Mathf.Sin((float)(diffLat / 2)) +
                   Mathf.Cos((float)_point.Item1) * Mathf.Cos((float)lat2) *
                   Mathf.Sin((float)(diffLon / 2)) * Mathf.Sin((float)(diffLon / 2));

        double angularDistance = 2 * Mathf.Asin(Mathf.Sqrt((float)a));

        return earthRadius * angularDistance;
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _finishButton.onClick.RemoveListener(FinishGame);
    }
}
