using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KahootScoreView : MonoBehaviour
{
    [SerializeField] private List<KahootBar> _kahootBars;

    public void SubmitScores(List<KahootAnswer> playerScores)
    {
        int maxIndex = playerScores.Max(a => a.AnswerIndex);

        List<int> counts = Enumerable.Range(0, maxIndex + 1)
            .Select(i => playerScores.Count(a => a.AnswerIndex == i))
            .ToList();

        for (int i = 0; i < counts.Count; i++)
        {
            if (i > _kahootBars.Count - 1)
            {
                return;
            }

            _kahootBars[i].SubmitScore(counts[i]);
        }
    }

}
