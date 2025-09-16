using TMPro;
using UnityEngine;

public class MapMarker : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _yPadding = 5f;

    private Camera _camera;
    private GameObject _objectToRenderLineTowards;

    public void Initialize(Camera camera, string text)
    {
        _camera = camera;
        _text.text = text;
    }

    public void RenderLineTowards(GameObject obj)
    {
        _objectToRenderLineTowards = obj;
    }

    private void Update()
    {
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);

        // _lineRenderer.SetPosition(0, transform.localPosition);
        //
        // Vector3 position = new Vector3
        //     (_objectToRenderLineTowards.transform.position.x,
        //         _objectToRenderLineTowards.transform.position.y + _yPadding,
        //         _objectToRenderLineTowards.transform.position.z);
        //
        // _lineRenderer.SetPosition(1, position);
    }
}
