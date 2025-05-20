using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbstactDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstactDungeonGenerator _dungeonGenerator;

    private void Awake()
    {
        _dungeonGenerator = (AbstactDungeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Dungeon"))
        {
            _dungeonGenerator.GenerateDungeon();
        }
    }
}
