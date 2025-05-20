using UnityEngine;

public abstract class AbstactDungeonGenerator : MonoBehaviour
{
    [SerializeField] protected FloorVisualizer _floorVisualizer;
    [SerializeField] protected Vector2Int _startPosition = Vector2Int.zero;

    public void GenerateDungeon()
    {
        _floorVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
