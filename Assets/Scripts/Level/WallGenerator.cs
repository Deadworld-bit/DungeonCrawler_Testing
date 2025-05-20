using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void GenerateWalls(HashSet<Vector2Int> floorPositions, FloorVisualizer floorVisualizer)
    {
        var wallList = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        floorVisualizer.InstantiateWalls(wallList);
    }

    private static List<(Vector2Int floorPosition, Vector2Int direction)> FindWallsInDirections(
        HashSet<Vector2Int> floorPositions, List<Vector2Int> directions)
    {
        List<(Vector2Int, Vector2Int)> wallList = new List<(Vector2Int, Vector2Int)>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directions)
            {
                var neighborPosition = position + direction;
                if (!floorPositions.Contains(neighborPosition))
                {
                    wallList.Add((position, direction));
                }
            }
        }
        return wallList;
    }
}