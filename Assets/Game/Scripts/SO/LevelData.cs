using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public GameObject levelPrefab;
    public float duration = 180f;
    public float spawnInterval = 2f;
    public GameObject bossZombiePrefab;
}
