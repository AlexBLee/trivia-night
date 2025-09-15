using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameSelectionContainer;
    [SerializeField] private GameObject _endGameContainer;
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

    public void ShowEndGameScreen(bool show)
    {
        _gameSelectionContainer.gameObject.SetActive(!show);
        _endGameContainer.gameObject.SetActive(show);
    }
}
