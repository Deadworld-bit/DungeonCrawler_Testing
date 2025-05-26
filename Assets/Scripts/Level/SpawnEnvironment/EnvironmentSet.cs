using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabWithWeight
{
    public GameObject prefab;
    public float weight;  //probability
}

[System.Serializable]
public class WallPrefab : PrefabWithWeight
{
    public float spawnHeight;
    public float additionalRotation;
}

[System.Serializable]
public class EnvironmentSet
{
    public string name;
    public List<PrefabWithWeight> floorPrefabs;
    public List<WallPrefab> wallPrefabs;
}