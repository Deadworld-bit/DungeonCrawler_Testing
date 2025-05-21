using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkGenerator : AbstactDungeonGenerator
{
    [SerializeField] protected RandomWalkMapData _mapData;

    /// Generates a dungeon using the random walk algorithm and visualizes the result.
    protected override void RunProceduralGeneration()
    {
        _floorVisualizer.Clear();
        HashSet<Vector2Int> floorPositions = RunRandomWalk(_mapData, _startPosition);
        _floorVisualizer.InstantiateFloorTitles(floorPositions);
        WallGenerator.GenerateWalls(floorPositions, _floorVisualizer);
    }

    /// <summary>
    /// Performs a random walk to generate a set of floor positions based on provided parameters.
    /// </summary>
    /// <param name="parameters">Configuration data for the random walk.</param>
    /// <param name="position">Starting position for the walk.</param>
    /// <returns>A set of Vector2Int positions representing the floor tiles.</returns>
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