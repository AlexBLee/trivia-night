using System;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;

public class SpawnMarkersOnMap : MonoBehaviour
{
	[SerializeField] private AbstractMap _map;
	[SerializeField] private Camera _mapCamera;
	[SerializeField] private float _spawnScale = 100f;
	[SerializeField] private MapMarker _mapMarker;

	private List<GameObject> _spawnedObjects = new();
	private List<Vector2d> _markerPositions = new();

	public void SetMarker(string coordinate, string teamName = "")
	{
		Vector2d vectorCoordinate = Conversions.StringToLatLon(coordinate);
		SpawnMarker(vectorCoordinate, teamName);
	}

	public void SetMarker(Vector2d coordinate, string teamName = "")
	{
		SpawnMarker(coordinate, teamName);
	}

	private void SpawnMarker(Vector2d coordinate, string teamName = "")
	{
		var mapMarker = Instantiate(_mapMarker);
		mapMarker.Initialize(_mapCamera, teamName);
		mapMarker.transform.localPosition = _map.GeoToWorldPosition(coordinate, true);
		mapMarker.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

		_markerPositions.Add(coordinate);
		_spawnedObjects.Add(mapMarker.gameObject);

		mapMarker.RenderLineTowards(_spawnedObjects[0]);
	}

	private void Update()
	{
		int count = _spawnedObjects.Count;
		for (int i = 0; i < count; i++)
		{
			var spawnedObject = _spawnedObjects[i];
			var location = _markerPositions[i];
			spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
			spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
		}
	}

	private void OnDisable()
	{
		foreach (var spawnedObject in _spawnedObjects)
		{
			Destroy(spawnedObject);
		}

		_spawnedObjects.Clear();
		_markerPositions.Clear();
	}
}
