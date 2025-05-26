using System.Collections.Generic;
using UnityEngine;

public class FloorVisualizer : MonoBehaviour
{
    [SerializeField] private Transform _floorParent;
    [SerializeField] private Transform _wallParent;

    [Header("Environment Settings")]
    [SerializeField] private List<EnvironmentSet> _environments;
    [SerializeField] private int _currentEnvironmentIndex = 0;

    [Header("General Settings")]
    [SerializeField] private float _cellSize = 1f;

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
        if (_environments == null || _currentEnvironmentIndex < 0 || _currentEnvironmentIndex >= _environments.Count)
        {
            Debug.LogError("Invalid environment settings. Please assign environments and set a valid index.");
            return;
        }

        EnvironmentSet currentEnv = _environments[_currentEnvironmentIndex];
        if (currentEnv.floorPrefabs == null || currentEnv.floorPrefabs.Count == 0)
        {
            Debug.LogWarning($"No floor prefabs defined for environment: {currentEnv.name}");
            return;
        }

        foreach (var position in floorPositions)
        {
            Vector3 worldPosition = new Vector3(position.x * _cellSize, 0, position.y * _cellSize);
            var selectedFloor = SelectWeightedRandom(currentEnv.floorPrefabs);
            if (selectedFloor != null)
            {
                Instantiate(selectedFloor.prefab, worldPosition, Quaternion.identity, _floorParent);
            }
        }
    }

    public void InstantiateWalls(List<(Vector2Int floorPosition, Vector2Int direction)> wallList)
    {
        // Validate environment settings
        if (_environments == null || _currentEnvironmentIndex < 0 || _currentEnvironmentIndex >= _environments.Count)
        {
            Debug.LogError("Invalid environment settings. Please assign environments and set a valid index.");
            return;
        }

        EnvironmentSet currentEnv = _environments[_currentEnvironmentIndex];
        if (currentEnv.wallPrefabs == null || currentEnv.wallPrefabs.Count == 0)
        {
            Debug.LogWarning($"No wall prefabs defined for environment: {currentEnv.name}");
            return;
        }

        foreach (var (floorPosition, direction) in wallList)
        {
            var selectedWall = SelectWeightedRandom(currentEnv.wallPrefabs);
            if (selectedWall != null)
            {
                Vector3 position = new Vector3(
                    (floorPosition.x + direction.x * 0.5f) * _cellSize,
                    selectedWall.spawnHeight,
                    (floorPosition.y + direction.y * 0.5f) * _cellSize
                );

                Vector3 dir3D = new Vector3(direction.x, 0, direction.y);
                Quaternion lookRotation = Quaternion.LookRotation(dir3D);
                Quaternion additionalRotation = Quaternion.Euler(0, selectedWall.additionalRotation, 0);
                Quaternion finalRotation = lookRotation * additionalRotation;

                Instantiate(selectedWall.prefab, position, finalRotation, _wallParent);
            }
        }
    }

    private T SelectWeightedRandom<T>(List<T> items) where T : PrefabWithWeight
    {
        if (items == null || items.Count == 0) return null;

        float totalWeight = 0;
        foreach (var item in items)
        {
            totalWeight += item.weight;
        }

        if (totalWeight <= 0) return null;

        float randomValue = Random.Range(0, totalWeight);
        float cumulative = 0;

        foreach (var item in items)
        {
            cumulative += item.weight;
            if (cumulative >= randomValue)
            {
                return item;
            }
        }

        return items[items.Count - 1];
    }
}