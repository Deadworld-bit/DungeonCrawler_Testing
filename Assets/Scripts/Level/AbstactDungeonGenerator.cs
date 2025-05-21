using UnityEngine;

public abstract class AbstactDungeonGenerator : MonoBehaviour
{
    [SerializeField] protected FloorVisualizer _floorVisualizer;
    [SerializeField] protected Vector2Int _startPosition = Vector2Int.zero;

    /// Initiates the dungeon generation process by clearing the visualizer and running the generation algorithm.
    public void GenerateDungeon()
    {
        _floorVisualizer.Clear();
        RunProceduralGeneration();
    }

    /// Abstract method to be implemented by subclasses to define specific dungeon generation logic.
    protected abstract void RunProceduralGeneration();
}