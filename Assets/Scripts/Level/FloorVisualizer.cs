using System.Collections.Generic;
using UnityEngine;

public class FloorVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject _floorPrefab;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private Transform _floorParent;
    [SerializeField] private Transform _wallParent;
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private float _wallSpawnHeight = 2f;
    [SerializeField] private float _wallSpawnRotation = 90f;

    public void Clear()
    {
        foreach (Transform child in _floorParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _wallParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void InstantiateFloorTitles(IEnumerable<Vector2Int> floorPositions)
    {
        foreach (var position in floorPositions)
        {
            Vector3 worldPosition = new Vector3(position.x * _cellSize, 0, position.y * _cellSize);
            Instantiate(_floorPrefab, worldPosition, Quaternion.identity, _floorParent);
        }
    }

    public void InstantiateWalls(List<(Vector2Int floorPosition, Vector2Int direction)> wallList)
    {
        foreach (var (floorPosition, direction) in wallList)
        {
            Vector3 position = new Vector3(
                (floorPosition.x + direction.x * 0.5f) * _cellSize,
                _wallSpawnHeight,
                (floorPosition.y + direction.y * 0.5f) * _cellSize
            );

            Vector3 dir3D = new Vector3(direction.x, 0, direction.y);

            Quaternion lookRotation = Quaternion.LookRotation(dir3D);
            Quaternion additionalRotation = Quaternion.Euler(0, _wallSpawnRotation, 0);
            Quaternion finalRotation = lookRotation * additionalRotation;
            Instantiate(_wallPrefab, position, finalRotation, _wallParent);
        }
    }
}