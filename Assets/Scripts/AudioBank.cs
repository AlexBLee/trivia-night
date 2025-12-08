using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioBank", menuName = "Scriptable Objects/AudioBank")]
public class AudioBank : ScriptableObject
{
    public SerializedDictionary<string, AudioClip> _audioClips;

    public AudioClip GetAudioClip(string key)
    {
        return _audioClips.ContainsKey(key)
            ? _audioClips[key]
            : null;
    }
}
