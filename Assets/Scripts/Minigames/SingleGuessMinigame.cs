using System;
using Cysharp.Threading.Tasks;
using Fleck;

public abstract class SingleGuessMinigame : Minigame, ISingleGuessGame
{
    public Action<Team, int> OnGuessClicked { get; set; }

    protected override async void ReceiveMessage(IWebSocketConnection socket, string message)
    {
        base.ReceiveMessage(socket, message);

        if (message == "button_clicked")
        {
            SendMessageToServer("disable");

            await UniTask.SwitchToMainThread();

            var team = _teamManager.GetTeam(socket);
            OnGuessClicked?.Invoke(team, _points);
        }
    }
}