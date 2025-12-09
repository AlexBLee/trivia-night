using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] private EndGameStageSetter _endGameStageSetter;
    [SerializeField] private TeamLabel _teamLabelPrefab;
    [SerializeField] private Transform _teamLabelsParent;
    [SerializeField] private TeamManager _teamManager;

    private Queue<int> _teamIndexes = new();
    private List<TeamLabel> _teamLabels = new();

    public void OnEnable()
    {
        var teamList = _teamManager.Teams
            .OrderBy(t => t.Value.CurrentScore)
            .ToList();

        for (int i = 0; i < teamList.Count; i++)
        {
            var team = teamList[i].Value;

            _endGameStageSetter.AddPlacement(i, team);
        }
    }
}
