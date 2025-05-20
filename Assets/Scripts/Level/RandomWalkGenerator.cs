using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkGenerator : AbstactDungeonGenerator
{
    [SerializeField] private RandomWalkMapData _mapData;
    protected override void RunProceduralGeneration()
    {
        _floorVisualizer.Clear();
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        _floorVisualizer.InstantiateFloorTitles(floorPositions);
        WallGenerator.GenerateWalls(floorPositions, _floorVisualizer);
        // foreach (var position in floorPositions)
        // {
        //     Debug.Log(position);
        // }
    }

    private HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = _startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < _mapData._interations; i++)
        {
            var path = ProceduralGenerationAlgorithm.RandomWalk(currentPosition, _mapData._walkLength);
            floorPositions.UnionWith(path);
            if (_mapData._startRandomlyEachInteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
