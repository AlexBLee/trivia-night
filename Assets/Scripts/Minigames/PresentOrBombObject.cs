using System;
using System.Collections;
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
            StartCoroutine(StartHiding());
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

    private IEnumerator StartHiding()
    {
        if (_explosionImage == null)
        {
            yield return null;
        }

        _explosionImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        _explosionImage.gameObject.SetActive(false);
        _presentImage.enabled = false;
    }

    private void OnDestroy()
    {
        StopCoroutine(StartHiding());
    }
}
