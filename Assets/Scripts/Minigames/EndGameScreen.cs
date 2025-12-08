using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] private TeamLabel _teamLabelPrefab;
    [SerializeField] private Transform _teamLabelsParent;
    [SerializeField] private TeamManager _teamManager;

    private Queue<int> _teamIndexes = new();
    private List<TeamLabel> _teamLabels = new();

    public void OnEnable()
    {
        var teamList = _teamManager.Teams.ToList();

        for (int i = 0; i < teamList.Count; i++)
        {
            var teamLabel = Instantiate(_teamLabelPrefab, _teamLabelsParent);
            teamLabel.AssignTeamAttributes(teamList[i].Value);
            _teamLabels.Add(teamLabel);
        }

        _teamIndexes = new Queue<int>(teamList
            .Select((team, index) => new {Team = team, Index = index})
            .OrderBy(team => team.Team.Value.CurrentScore)
            .Select(team => team.Index));
    }

    void Update()
    {
        if (_teamIndexes.Count > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            _teamLabels[_teamIndexes.Dequeue()].StopDisplay();
        }
    }
}
