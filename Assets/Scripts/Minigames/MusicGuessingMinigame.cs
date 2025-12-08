using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicGuessingMinigame : SingleGuessMinigame
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<Button> _playAudioButtons;
    [SerializeField] private Button _finishButton;

    private List<AudioClip> _audioClips = new();
    public Action<Team, int> OnGuessClicked { get; set; }

    public override void Initialize(MinigameData minigameData)
    {
        base.Initialize(minigameData);

        _audioClips.Clear();

        string[] audioClips = minigameData.Input.Split(',');
        for (int i = 0; i < audioClips.Length; i++)
        {
            var clip = Resources.Load<AudioClip>(audioClips[i]);
            _audioClips.Add(clip);
        }

        for (int i = 0; i < _playAudioButtons.Count; i++)
        {
            if (i > _audioClips.Count - 1)
            {
                break;
            }

            var audioIndex = i;
            _playAudioButtons[i].onClick.AddListener(() => PlayAudioClip(_audioClips[audioIndex]));
        }

        SendMessageToServer("music");
        _finishButton.onClick.AddListener(FinishGame);
    }

    private void PlayAudioClip(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        foreach (var audioButton in _playAudioButtons)
        {
            audioButton.onClick.RemoveAllListeners();
        }

        _finishButton.onClick.RemoveListener(FinishGame);
    }
}
