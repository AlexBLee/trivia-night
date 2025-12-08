using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioBank _audioBank;

    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _sfxAudioSource;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayMusic(string key)
    {
        var music = _audioBank.GetAudioClip(key);
        if (music == null)
        {
            Debug.LogWarning("No music by that key! " + key);
            return;
        }

        _musicAudioSource.volume = 1f;
        _musicAudioSource.clip = music;
        _musicAudioSource.Play();
    }

    public void PlaySfx(string key)
    {
        var sfx = _audioBank.GetAudioClip(key);
        if (sfx == null)
        {
            Debug.LogWarning("No sfx by that key! " + key);
            return;
        }

        _sfxAudioSource.PlayOneShot(sfx);
    }

    public void FadeOutMusic(float duration)
    {
        StartCoroutine(FadeOutMusicAndStop(duration));
    }

    private IEnumerator FadeOutMusicAndStop(float duration)
    {
        float startVolume = _musicAudioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        _musicAudioSource.volume = 0f;
        _musicAudioSource.Stop();
    }

    public void StopMusic()
    {
        _musicAudioSource.Stop();
    }

    public void StopAllSfx()
    {
        _sfxAudioSource.Stop();
    }
}
