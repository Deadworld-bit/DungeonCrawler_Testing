using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstGenerator : RandomWalkGenerator
{
    [SerializeField] private int _corridorLength = 14;
    [SerializeField] private int _corridorCount = 5;
    [SerializeField][Range(0.1f, 1f)] private float _roomPercentage;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);
        floorPositions.UnionWith(roomPositions);

        _floorVisualizer.InstantiateFloorTitles(floorPositions);
        WallGenerator.GenerateWalls(floorPositions, _floorVisualizer);
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> roomPositions)
    {
        var currentPosition = _startPosition;
        roomPositions.Add(currentPosition);

        for (int i = 0; i < _corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithm.CorridorFirst(currentPosition, _corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            roomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomCount = Mathf.RoundToInt(potentialRoomPositions.Count * _roomPercentage);

        List<Vector2Int> roomList = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomCount).ToList();
        foreach (var roomPosition in roomList)
        {
            var roomFloor = RunRandomWalk(_mapData, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }
}
