using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioBank _audioBank;

    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _sfxAudioSource;

    public void PlayMusic(string key)
    {
        var music = _audioBank.GetAudioClip(key);
        if (music == null)
        {
            Debug.LogWarning("No music by that key! " + key);
            return;
        }

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

    public void StopMusic()
    {
        _musicAudioSource.Stop();
    }

    public void StopAllSfx()
    {
        _sfxAudioSource.Stop();
    }
}
