using UnityEngine;
using System.Collections.Generic;

public static class ProceduralGenerationAlgorithm
{
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


