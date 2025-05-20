using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public static class ProceduralGenerationAlgorithm
{
    // Random walk algorithm to generate map
    public static HashSet<Vector2Int> RandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        var previousPosistion = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousPosistion + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosistion = newPosition;
        }

        return path;
    }

    public static List<Vector2Int> CorridorFirst(Vector2Int startPosition, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPosition = startPosition;
        corridor.Add(currentPosition);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }

        return corridor;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1),  //up
        new Vector2Int(1,0),  //right
        new Vector2Int(0,-1), //down
        new Vector2Int(-1,0)  //left
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}


