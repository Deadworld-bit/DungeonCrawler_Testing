using UnityEngine;

[CreateAssetMenu(fileName = "RandomWalkParameters", menuName = "RMG/RandomWalkMapData")]
public class RandomWalkMapData : ScriptableObject
{
    public int _interations = 10;
    public int _walkLength = 10;
    public bool _startRandomlyEachInteration = true;
}
