using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    [SerializeField] private CharacterGetter _characterGetter;
    [SerializeField] private TeamDisplay teamDisplayPrefab;
    [SerializeField] private GameObject _layoutGroupParent;

    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private TextMeshProUGUI _ipText;

    private List<TeamDisplay> _teamTexts = new();
    private int _defaultNumberOfLabels = 4;

    private void Start()
    {
        _startButton.onClick.AddListener(StartGame);
        _ipText.text = $"{ServerExtensions.GetLocalIpv4Address()}:8081";

        CreateTeamLabels(_defaultNumberOfLabels);
    }

    public void CreateTeamLabels(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var label = Instantiate(teamDisplayPrefab, _layoutGroupParent.transform);
            _teamTexts.Add(label);
        }
    }

    public void AddStartGameCallback(UnityAction callback)
    {
        _startButton.onClick.AddListener(callback);
    }

    public void ChangeTeamDisplay(int index, string nameText, Color color, string characterName = "")
    {
        if (index > _teamTexts.Count - 1)
        {
            return;
        }

        var teamLabel = _teamTexts[index];

        teamLabel.ChangeLabel(nameText, color);
        teamLabel.SetImage(_characterGetter.GetCharacterSprite(characterName));
    }

    private void StartGame()
    {
        gameObject.SetActive(false);
        _gamePanel.SetActive(true);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveAllListeners();

        foreach (var label in _teamTexts)
        {
            Destroy(label.gameObject);
        }
        _teamTexts.Clear();
    }
}
