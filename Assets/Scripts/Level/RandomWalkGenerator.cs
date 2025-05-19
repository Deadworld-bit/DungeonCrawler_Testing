using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int _startPosition = Vector2Int.zero;
    [SerializeField] private int _interations = 10;
    [SerializeField] private int _walkLength = 10;
    [SerializeField] private bool _startRandomlyEachInteration = true;

    [SerializeField] private FloorVisualizer _floorVisualizer;

    public void RunProceduralGeneration()
    {
        _floorVisualizer.Clear(); 
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        _floorVisualizer.PaintFloorTitles(floorPositions);
        foreach (var position in floorPositions)
        {
            Debug.Log(position);
        }
    }

    private HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = _startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < _interations; i++)
        {
            var path = ProceduralGenerationAlgorithm.RandomWalk(currentPosition, _walkLength);
            floorPositions.UnionWith(path);
            if (_startRandomlyEachInteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
