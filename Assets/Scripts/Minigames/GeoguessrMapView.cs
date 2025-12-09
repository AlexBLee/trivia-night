using System;
using System.Collections;
using System.Collections.Generic;
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
    private List<Vector2d> _points = new();

    public void Initialize(string[] coords)
    {
        var latitude = Convert.ToDouble(coords[0]);
        var longitude = Convert.ToDouble(coords[1]);
        _point = new Vector2d(latitude, longitude);
        _points.Clear();

        _spawnMarkersOnMap.ClearPoints();
        _map.SetCenterLatitudeLongitude(_point);
        SpawnMarkerOnMap(_point);
    }

    public void SpawnMarkerOnMap(Vector2d point, string teamName = "", string characterName = "")
    {
        _spawnMarkersOnMap.SetMarker(point, teamName, characterName);
        _points.Add(point);
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

    private Vector2dBounds GetBoundsFromPoints()
    {
        if (_points.Count == 0)
        {
            return new Vector2dBounds();
        }

        double minLat = double.MaxValue;
        double maxLat = double.MinValue;
        double minLon = double.MaxValue;
        double maxLon = double.MinValue;

        foreach (var point in _points)
        {
            minLat = Math.Min(minLat, point.x);
            maxLat = Math.Max(maxLat, point.x);
            minLon = Math.Min(minLon, point.y);
            maxLon = Math.Max(maxLon, point.y);
        }

        Vector2d southwest = new Vector2d(minLat, minLon);
        Vector2d northeast = new Vector2d(maxLat, maxLon);

        return new Vector2dBounds(southwest, northeast);
    }

    public void FitCameraBounds()
    {
        var bounds = GetBoundsFromPoints();
        var center = bounds.Center;

        _map.SetCenterLatitudeLongitude(center);
        _map.SetZoom(GetZoomToFit(bounds));
        _map.UpdateMap();
    }

    private int GetZoomToFit(Vector2dBounds bounds)
    {
        double latSpan = bounds.North - bounds.South;
        double lonSpan = bounds.East - bounds.West;

        double maxSpan = Mathf.Max((float)latSpan, (float)lonSpan);

        double zoom = Math.Log(360.0 / maxSpan, 2);
        return Mathf.Clamp((int)zoom + 1, 0, 20);
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
