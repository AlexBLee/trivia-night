using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<Sprite> _cardSprites;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _cardImage;
    [SerializeField] private Image _cardBack;

    private float _rotateDegrees = 90f;
    private float _rotateSpeed = .5f;

    private void Start()
    {
        _cardImage.sprite = _cardSprites[Random.Range(0, _cardSprites.Count)];
        _cardImage.enabled = false;
        _cardBack.enabled = true;
    }

    public void ShowFace()
    {
        _cardImage.enabled = true;
        _cardBack.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var sequence = DOTween.Sequence();

        sequence.Append(_rectTransform.DOLocalRotate(new Vector3(0f, _rotateDegrees, 0f), _rotateSpeed).OnComplete(() =>
            {
                _cardBack.enabled = false;
                _cardImage.enabled = true;
            }));

        sequence.Append(_rectTransform.DOLocalRotate(new Vector3(0f, 0f, 0f), _rotateSpeed));

        sequence.Play();
    }
}
