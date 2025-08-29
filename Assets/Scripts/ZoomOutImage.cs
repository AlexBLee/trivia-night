using System;
using UnityEngine;
using UnityEngine.UI;

public class ZoomOutImage : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _zoomStep = 0.1f;
    private float _currentScale = 15f;

    void Start()
    {
        ApplyZoom();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnZoomOutButton();
        }
    }

    private void OnZoomOutButton()
    {
        // reduce scale but clamp so it never goes below 1 (normal size)
        _currentScale = Mathf.Max(1f, _currentScale - _zoomStep);
        ApplyZoom();
    }

    private void ApplyZoom()
    {
        _image.rectTransform.localScale = new Vector3(_currentScale, _currentScale, 1f);
    }
}