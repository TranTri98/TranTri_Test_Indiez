using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPrefabs;
    [SerializeField] private int amountToSpawn = 20;
    [SerializeField] private Transform groundPlane;
    [SerializeField] private Terrain terrain;


    [ContextMenu("Generate Objects Plan")]
    private void GenerateInEditor()
    {
        if (Application.isPlaying) return;

        ClearSpawned();

        Vector3 center = groundPlane.position;
        Vector3 scale = groundPlane.localScale;

        float width = 10 * scale.x;
        float height = 10 * scale.z;

        for (int i = 0; i < amountToSpawn; i++)
        {
            float x = Random.Range(center.x - width / 2f, center.x + width / 2f);
            float z = Random.Range(center.z - height / 2f, center.z + height / 2f);
            Vector3 spawnPos = new Vector3(x, center.y + 0.15f , z);

            GameObject prefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

            float randomRotationY = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(0f, randomRotationY, 0f);

            GameObject obj = Instantiate(prefab, spawnPos, rotation, this.transform);
            obj.name = $"Generated_{i}";
        }
    }

    [ContextMenu("Generate Objects Terrain")]
    private void GenerateInEditorTerrain()
    {
        if (Application.isPlaying) return;

        ClearSpawned();

        if (terrain == null)
        {
            Debug.LogWarning("Terrain not assigned.");
            return;
        }

        Vector3 terrainPos = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        for (int i = 0; i < amountToSpawn; i++)
        {
            float x = Random.Range(terrainPos.x, terrainPos.x + terrainSize.x);
            float z = Random.Range(terrainPos.z, terrainPos.z + terrainSize.z);

            float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrainPos.y;

            Vector3 spawnPos = new Vector3(x, y, z);

            GameObject prefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

            float randomRotationY = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(0f, randomRotationY, 0f);

            GameObject obj = Instantiate(prefab, spawnPos, rotation, this.transform);
            obj.name = $"Generated_{i}";
        }
    }

    // Optional: Remove old generated objects
    private void ClearSpawned()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
