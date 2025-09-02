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
}

public class MinigameSpawner : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<MinigameType, Minigame> _minigames;
    [SerializeField] private TextAsset _minigameJson;

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

    public void OpenMinigame(MinigameType type)
    {
        var minigame = _minigames[type];

        minigame.gameObject.SetActive(true);
        _minigames[type].Initialize();
    }
}
