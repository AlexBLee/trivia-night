using System;
using UnityEngine;

public interface ISingleGuessGame
{
    public Action<Team, int> OnGuessClicked { get; set; }
}
