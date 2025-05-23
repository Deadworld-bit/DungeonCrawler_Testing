using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Numerics;

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

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomList = new List<BoundsInt>();
        roomQueue.Enqueue(spaceToSplit);
        while (roomQueue.Count > 0)
        {
            var room = roomQueue.Dequeue();
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if (Random.value < 0.5f)
                {
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomQueue, room);
                    }
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomQueue, room);
                    }
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomList.Add(room);
                    }
                }
            }
        }

        return roomList;
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomQueue, BoundsInt room)
    {
        var ySplit = Random.Range(1, room.size.y);
        BoundsInt firstRoom = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt secondRoom = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
        new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));

        roomQueue.Enqueue(firstRoom);
        roomQueue.Enqueue(secondRoom);
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomQueue, BoundsInt room)
    {
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt firstRoom = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt secondRoom = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
        new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));

        roomQueue.Enqueue(firstRoom);
        roomQueue.Enqueue(secondRoom);
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


