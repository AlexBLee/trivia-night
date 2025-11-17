using System.IO;
using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine;

[System.Serializable]
public class MinigameData
{
    [JsonConverter(typeof(StringEnumConverter))]
    public MinigameType Type { get; set; }
    public string Input { get; set; }
    public string Answer { get; set; }
}

public class MinigameSpawner : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<MinigameType, Minigame> _minigames;

    private MinigameData[] _minigameData;

    public void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "minigames.json");
        if (File.Exists(filePath))
        {
            string buffer = File.ReadAllText(filePath);
            JArray jArray = JArray.Parse(buffer);

            _minigameData = jArray.ToObject<MinigameData[]>();
        }
    }

    public void OpenMinigame(int index)
    {
        MinigameData minigameData = _minigameData[index];
        var minigame = _minigames[minigameData.Type];

        minigame.gameObject.SetActive(true);
        _minigames[minigameData.Type].Initialize(minigameData);
    }
}
