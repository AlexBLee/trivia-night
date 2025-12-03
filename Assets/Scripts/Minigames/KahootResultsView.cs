using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KahootResultsView : MonoBehaviour
{
    [SerializeField] private TeamAnswerLabel _teamAnswerLabel;
    [SerializeField] private Transform _teamResultsContainer;

    private List<TeamAnswerLabel> _teamAnswerLabels = new List<TeamAnswerLabel>();

    public void CalculateScore(List<KahootAnswer> teamAnswers)
    {
        var sortedAnswers = teamAnswers.OrderByDescending(answer => answer.Score).ToList();

        foreach (var kahootAnswer in sortedAnswers)
        {
            var teamAnswerLabel = Instantiate(_teamAnswerLabel, _teamResultsContainer);
            _teamAnswerLabels.Add(teamAnswerLabel);
            teamAnswerLabel.SetLabel(kahootAnswer.Team.TeamName, kahootAnswer.Score);
        }
    }

    private void OnDisable()
    {
        foreach (var teamAnswerLabel in _teamAnswerLabels)
        {
            Destroy(teamAnswerLabel.gameObject);
        }
        _teamAnswerLabels.Clear();
    }
}
