using System.Collections.Generic;
using UnityEngine;

public class FloorVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject _floorPrefab;    
    [SerializeField] private Transform _floorParent;     
    [SerializeField] private float _cellSize = 1f;       

    public void Clear()
    {
        foreach (Transform child in _floorParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void PaintFloorTitles(IEnumerable<Vector2Int> floorPositions)
    {
        foreach (var position in floorPositions)
        {
            Vector3 worldPosition = new Vector3(position.x * _cellSize, 0, position.y * _cellSize);
            Instantiate(_floorPrefab, worldPosition, Quaternion.identity, _floorParent);
        }
    }
}