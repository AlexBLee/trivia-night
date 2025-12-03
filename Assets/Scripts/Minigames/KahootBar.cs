using TMPro;
using UnityEngine;

public class KahootBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private RectTransform _rectTransform;

    [SerializeField] private float _addedHeight = 50f;

    private float _defaultHeight = 10f;

    public void SubmitScore(int total)
    {
        _text.text = total.ToString();

        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x,  Mathf.Max(_defaultHeight, _addedHeight * total));
    }
}
