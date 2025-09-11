using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameSelectionContainer;

    public void ShowGameSelection(bool show)
    {
        _gameSelectionContainer.gameObject.SetActive(show);
    }
}
