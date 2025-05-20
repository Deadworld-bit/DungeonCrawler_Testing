using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkGenerator : AbstactDungeonGenerator
{
    [SerializeField] protected RandomWalkMapData _mapData;

    protected override void RunProceduralGeneration()
    {
        _floorVisualizer.Clear();
        HashSet<Vector2Int> floorPositions = RunRandomWalk(_mapData, _startPosition);
        _floorVisualizer.InstantiateFloorTitles(floorPositions);
        WallGenerator.GenerateWalls(floorPositions, _floorVisualizer);
        // foreach (var position in floorPositions)
        // {
        //     Debug.Log(position);
        // }
    }

    protected HashSet<Vector2Int> RunRandomWalk(RandomWalkMapData parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < parameters._interations; i++)
        {
            var path = ProceduralGenerationAlgorithm.RandomWalk(currentPosition, parameters._walkLength);
            floorPositions.UnionWith(path);
            if (parameters._startRandomlyEachInteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
