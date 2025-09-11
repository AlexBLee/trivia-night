using TMPro;
using UnityEngine;

public class MapMarker : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    private Camera _camera;

    public void Initialize(Camera camera, string text)
    {
        _camera = camera;
        _text.text = text;
    }

    private void Update()
    {
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
    }
}
