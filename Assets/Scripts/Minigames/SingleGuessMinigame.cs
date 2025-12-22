using System;
using Cysharp.Threading.Tasks;
using Fleck;
using UnityEngine;

public abstract class SingleGuessMinigame : Minigame, ISingleGuessGame
{
    public Action<Team, int> OnGuessClicked { get; set; }

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);
        _uiManager.ShowCharacters();
    }

    protected override async void ReceiveMessage(IWebSocketConnection socket, string message)
    {
        base.ReceiveMessage(socket, message);

        if (message == "button_clicked")
        {
            SendMessageToServer("disable");

            await UniTask.SwitchToMainThread();

            AudioManager.Instance.PlaySfx("GenericBuzz");

            var team = _teamManager.GetTeam(socket);
            OnGuessClicked?.Invoke(team, _points);

            _uiManager.AnimateCharacter(team);
            _uiManager.DarkenAllCharactersExcept(team);
        }
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReenableGuess();
        }
    }

    private void ReenableGuess()
    {
        SendMessageToServer("reenable");
        _uiManager.BrightenAllCharacters();
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _uiManager.ShowCharacters(false);
    }
}