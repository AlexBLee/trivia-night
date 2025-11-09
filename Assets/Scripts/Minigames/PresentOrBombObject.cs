using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PresentOrBombObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _presentImage;
    [SerializeField] private GameObject _explosionImage;

    private bool _isBomb;

    public void SetBomb()
    {
        _isBomb = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isBomb)
        {
            if (_explosionImage == null)
            {
                return;
            }

            _explosionImage.gameObject.SetActive(true);
        }
        else
        {
            if (_presentImage == null)
            {
                return;
            }

            _presentImage.enabled = false;
        }

    }
}
