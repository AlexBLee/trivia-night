using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameSelectionContainer;
    [SerializeField] private GameObject[] _finalJeopardyButtons;

    public void ShowGameSelection(bool show)
    {
        _gameSelectionContainer.gameObject.SetActive(show);
    }

    public void ShowFinalJeopardy(bool show)
    {
        foreach (var button in _finalJeopardyButtons)
        {
            button.SetActive(show);
        }
    }
}
