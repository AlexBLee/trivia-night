using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicGuessingMinigame : SingleGuessMinigame
{
    [SerializeField] private List<Button> _playAudioButtons;
    [SerializeField] private Button _finishButton;
    [SerializeField] private TextMeshProUGUI _songText;

    private List<AudioClip> _audioClips = new();
    private AudioClip _fullAudioClip;

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);

        AudioManager.Instance.FadeOutMusic(1f);

        _songText.text = minigameData.Answer;
        _audioClips.Clear();

        string[] audioClips = minigameData.Input.Split(',');
        for (int i = 0; i < audioClips.Length; i++)
        {
            var clip = Resources.Load<AudioClip>($"MusicGuessing/{audioClips[i]}");

            if (i == audioClips.Length - 1)
            {
                _fullAudioClip = clip;
                break;
            }

            _audioClips.Add(clip);
        }

        for (int i = 0; i < _playAudioButtons.Count; i++)
        {
            if (i > _audioClips.Count - 1)
            {
                break;
            }

            var audioIndex = i;
            _playAudioButtons[i].onClick.AddListener(() => AudioManager.Instance.PlaySfx(_audioClips[audioIndex], 0.5f));
        }

        SendMessageToServer("question");
        _finishButton.onClick.AddListener(FinishGame);
    }

    private void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _songText.gameObject.SetActive(true);
            AudioManager.Instance.PlaySfx(_fullAudioClip, 0.5f);
        }
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        foreach (var audioButton in _playAudioButtons)
        {
            audioButton.onClick.RemoveAllListeners();
        }

        _finishButton.onClick.RemoveListener(FinishGame);
        _songText.gameObject.SetActive(false);
        AudioManager.Instance.StopAllSfx();
        AudioManager.Instance.PlayMusic("JeopardyMusic");
    }
}
