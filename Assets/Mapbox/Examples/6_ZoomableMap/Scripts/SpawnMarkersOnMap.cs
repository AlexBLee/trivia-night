namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnMarkersOnMap : MonoBehaviour
	{
		[SerializeField] private AbstractMap _map;
		[SerializeField] private float _spawnScale = 100f;
		[SerializeField] private GameObject _markerPrefab;

		private List<GameObject> _spawnedObjects = new();
		private List<Vector2d> _markerPositions = new();

		public void SetMarker(string coordinate)
		{
			Vector2d vectorCoordinate = Conversions.StringToLatLon(coordinate);
			_markerPositions.Add(vectorCoordinate);
			var instance = Instantiate(_markerPrefab);
			instance.transform.localPosition = _map.GeoToWorldPosition(vectorCoordinate, true);
			instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			_spawnedObjects.Add(instance);
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
}