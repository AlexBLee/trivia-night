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

	public void SetMarker(string coordinate)
	{
		Vector2d vectorCoordinate = Conversions.StringToLatLon(coordinate);
		SpawnMarker(vectorCoordinate);
	}

	public void SetMarker(Vector2d coordinate)
	{
		SpawnMarker(coordinate);
	}

	private void SpawnMarker(Vector2d coordinate)
	{
		_markerPositions.Add(coordinate);
		var instance = Instantiate(_mapMarker);
		instance.Initialize(_mapCamera, "AAAAA");
		instance.transform.localPosition = _map.GeoToWorldPosition(coordinate, true);
		instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
		_spawnedObjects.Add(instance.gameObject);
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
}
