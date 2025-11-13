using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _teamText;
    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private TextMeshProUGUI _ipText;

    private void Start()
    {
        _startButton.onClick.AddListener(StartGame);
        _ipText.text = $"{ServerExtensions.GetLocalIpv4Address()}:8081";
    }

    public void AddStartGameCallback(UnityAction callback)
    {
        _startButton.onClick.AddListener(callback);
    }

    public void ChangeLabel(int index, string nameText, Color color)
    {
        if (index > _teamText.Count - 1)
        {
            return;
        }

        var teamText = _teamText[index];

        teamText.color = color;
        teamText.text = nameText;
    }

    private void StartGame()
    {
        gameObject.SetActive(false);
        _gamePanel.SetActive(true);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveAllListeners();
    }
}
