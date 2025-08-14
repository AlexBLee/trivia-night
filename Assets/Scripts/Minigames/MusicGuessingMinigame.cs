using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicGuessingMinigame : Minigame
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _audioClips;
    [SerializeField] private List<Button> _playAudioButtons;

    public override void Initialize()
    {
        base.Initialize();

        for (int i = 0; i < _playAudioButtons.Count; i++)
        {
            if (i > _audioClips.Count - 1)
            {
                break;
            }

            var audioIndex = i;
            _playAudioButtons[i].onClick.AddListener(() => PlayAudioClip(_audioClips[audioIndex]));
        }
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        foreach (var audioButton in _playAudioButtons)
        {
            audioButton.onClick.RemoveAllListeners();
        }
    }

    private void PlayAudioClip(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }
}
