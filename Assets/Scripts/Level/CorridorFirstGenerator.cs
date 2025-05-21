using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstGenerator : RandomWalkGenerator
{
    [SerializeField] private int _corridorLength = 14;
    [SerializeField] private int _corridorCount = 5;
    [SerializeField][Range(0.1f, 1f)] private float _roomPercentage;

    /// Executes the corridor-first dungeon generation process.
    protected override void RunProceduralGeneration()
    {
        GenerateCorridorFirstDungeon();
    }

    /// Generates a dungeon by creating corridors first, followed by rooms at selected positions and dead ends,
    /// then expands corridors and visualizes the result.
    private void GenerateCorridorFirstDungeon()
    {
        var floorPositions = new HashSet<Vector2Int>();
        var potentialRoomPositions = new HashSet<Vector2Int>();
        var corridors = CreateCorridors(floorPositions, potentialRoomPositions);
        var roomPositions = CreateRooms(potentialRoomPositions);
        var deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnds(deadEnds, roomPositions);
        floorPositions.UnionWith(roomPositions);

        foreach (var corridor in corridors)
        {
            var expandedCorridor = ExpandCorridor(corridor);
            floorPositions.UnionWith(expandedCorridor);
        }

        _floorVisualizer.InstantiateFloorTitles(floorPositions);
        WallGenerator.GenerateWalls(floorPositions, _floorVisualizer);
    }

    /// <summary>
    /// Creates a series of connected corridors, updating floor and potential room positions.
    /// </summary>
    /// <param name="floorPositions">Set to store all corridor tiles.</param>
    /// <param name="potentialRoomPositions">Set to store corridor endpoints for potential rooms.</param>
    /// <returns>List of corridors, each a list of Vector2Int positions.</returns>
    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = _startPosition;
        potentialRoomPositions.Add(currentPosition);
        var corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < _corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithm.CorridorFirst(currentPosition, _corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor.Last();
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }

        return corridors;
    }

    /// <summary>
    /// Creates rooms at a subset of potential room positions using random walks.
    /// </summary>
    /// <param name="potentialRoomPositions">Set of positions where rooms can be created.</param>
    /// <returns>Set of Vector2Int positions representing room tiles.</returns>
    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        var roomPositions = new HashSet<Vector2Int>();
        int roomCount = Mathf.RoundToInt(potentialRoomPositions.Count * _roomPercentage);

        var selectedRooms = potentialRoomPositions.OrderBy(_ => Guid.NewGuid()).Take(roomCount);
        foreach (var roomPosition in selectedRooms)
        {
            var roomFloor = RunRandomWalk(_mapData, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    /// <summary>
    /// Identifies dead ends in the floor positions (positions with only one neighbor).
    /// </summary>
    /// <param name="floorPositions">Set of current floor tiles to check.</param>
    /// <returns>List of Vector2Int positions that are dead ends.</returns>
    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        var deadEnds = new List<Vector2Int>();

        foreach (var position in floorPositions)
        {
            int neighborCount = Direction2D.cardinalDirectionsList.Count(direction => floorPositions.Contains(position + direction));
            if (neighborCount == 1)
                deadEnds.Add(position);
        }

        return deadEnds;
    }

    /// <summary>
    /// Creates rooms at dead end positions if they don't already have rooms.
    /// </summary>
    /// <param name="deadEnds">List of dead end positions.</param>
    /// <param name="roomPositions">Set to store room tiles, updated with new rooms.</param>
    private void CreateRoomsAtDeadEnds(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomPositions)
    {
        foreach (var deadEnd in deadEnds)
        {
            if (!roomPositions.Contains(deadEnd))
            {
                var room = RunRandomWalk(_mapData, deadEnd);
                roomPositions.UnionWith(room);
            }
        }
    }

    /// <summary>
    /// Expands a corridor by adding tiles to the sides, handling corners with 3x3 blocks.
    /// </summary>
    /// <param name="corridor">List of Vector2Int positions forming the corridor.</param>
    /// <returns>List of Vector2Int positions representing the expanded corridor.</returns>
    private List<Vector2Int> ExpandCorridor(List<Vector2Int> corridor)
    {
        var expanded = new HashSet<Vector2Int>();
        Vector2Int lastDirection = Vector2Int.zero;

        if (corridor == null || corridor.Count < 2)
            return new List<Vector2Int>();

        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int direction = corridor[i] - corridor[i - 1];
            Vector2Int side = GetDirection90From(direction);

            if (lastDirection != Vector2Int.zero && direction != lastDirection)
            {
                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                        expanded.Add(corridor[i - 1] + new Vector2Int(x, y));

                expanded.Add(corridor[i]);
                expanded.Add(corridor[i] + side);
            }
            else
            {
                expanded.Add(corridor[i - 1]);
                expanded.Add(corridor[i - 1] + side);
            }
            lastDirection = direction;
        }
        expanded.Add(corridor.Last());

        return expanded.ToList();
    }

    /// <summary>
    /// Returns a direction 90 degrees clockwise from the given direction.
    /// </summary>
    /// <param name="direction">Input cardinal direction.</param>
    /// <returns>Vector2Int representing the perpendicular direction.</returns>
    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return Vector2Int.right;
        if (direction == Vector2Int.right) return Vector2Int.down;
        if (direction == Vector2Int.down) return Vector2Int.left;
        if (direction == Vector2Int.left) return Vector2Int.up;
        return Vector2Int.zero; 
    }
}