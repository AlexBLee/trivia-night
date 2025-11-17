using System;
using System.Collections;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

public class GeoguessrMapView : MonoBehaviour
{
    [SerializeField] private AbstractMap _map;
    [SerializeField] private Camera _mapCamera;
    [SerializeField] private SpawnMarkersOnMap _spawnMarkersOnMap;

    private int _maxPoints = 500;
    private float _maxDistance = 10;

    private float _zoomAmount = 16f;
    private float _zoomDuration = 2f;
    private float _timeBeforeZoom = 3f;

    private Vector2d _point;

    public void Initialize(string[] coords)
    {
        var latitude = Convert.ToDouble(coords[0]);
        var longitude = Convert.ToDouble(coords[1]);
        _point = new Vector2d(latitude, longitude);

        _map.SetCenterLatitudeLongitude(_point);
        _spawnMarkersOnMap.SetMarker(_point);
        _map.UpdateMap();
    }

    public void SpawnMarkerOnMap(string point, string teamName = "")
    {
        _spawnMarkersOnMap.SetMarker(point, teamName);
    }

    public int GetScore(string coord)
    {
        return GetScoreFromDistance(GetDistanceInKm(coord));
    }

    public void StartZoomingIn()
    {
        StartCoroutine(ZoomCoroutine(_zoomAmount, _zoomDuration));
    }

    private double GetDistanceInKm(string answer)
    {
        double lat1 = _point.x;

        var coords = answer.Split(',');
        double lat2 = Convert.ToDouble(coords[0]);
        double lon2 = Convert.ToDouble(coords[1]);

        double earthRadius = 6371.0;

        double diffLat = (lat2 - _point.x) * Mathf.Deg2Rad;
        double diffLon = (lon2 - _point.y) * Mathf.Deg2Rad;

        lat1 *= Mathf.Deg2Rad;
        lat2 *= Mathf.Deg2Rad;

        double a = Mathf.Sin((float)(diffLat / 2)) * Mathf.Sin((float)(diffLat / 2)) +
                   Mathf.Cos((float)lat1) * Mathf.Cos((float)lat2) *
                   Mathf.Sin((float)(diffLon / 2)) * Mathf.Sin((float)(diffLon / 2));

        double angularDistance = 2 * Mathf.Asin(Mathf.Sqrt((float)a));

        return earthRadius * angularDistance;
    }

    private int GetScoreFromDistance(double km)
    {
        if (km < 0.1f) return _maxPoints;
        if (km > _maxDistance) return 0;
        return Mathf.Max(0, (int)(_maxPoints * (1 - (km / _maxDistance))));
    }

    private IEnumerator ZoomCoroutine(float targetZoom, float duration)
    {
        float startZoom = _map.AbsoluteZoom;
        Vector2d coord = new Vector2d(_point.x, _point.y);
        float elapsed = 0f;

        yield return new WaitForSeconds(_timeBeforeZoom);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float easedT = Mathf.SmoothStep(0f, 1f, t);

            float currentZoom = Mathf.Lerp(startZoom, targetZoom, easedT);
            _map.UpdateMap(coord, currentZoom);

            yield return null;
        }

        _map.UpdateMap(coord, targetZoom);
    }
}
