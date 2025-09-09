using System.Collections.Generic;
using UnityEngine;

public class AdminPanel : MonoBehaviour
{
    [SerializeField] private List<TeamOptions> _teamOptions;

    void Start()
    {
#if !UNITY_EDITOR
        Display.displays[1].Activate();
#endif
    }

    public void AssignTeams(List<Team> team)
    {
        for (int i = 0; i < team.Count; i++)
        {
            _teamOptions[i].AssignTeam(team[i]);
        }
    }
}
