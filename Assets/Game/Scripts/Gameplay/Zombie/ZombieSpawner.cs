using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float spawnRadius = 30f;
    [SerializeField] private Transform parentSpawn;

    [Header("Ground Settings")]
    [SerializeField] private Transform ground;
    [SerializeField] private float groundY = 0.5f;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    private Transform player;
    private Coroutine spawnRoutine;

    public static ZombieSpawner I;

    private float spawnInterval = 2f;

    private bool hasSpawnedBoss = false;

    private void Awake()
    {
        if (I != null && I != this) Destroy(gameObject);
        else I = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (ground != null)
            groundY = ground.position.y + 0.5f;

        StartSpawning();
    }

    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = StartCoroutine(SpawnLoop());
        }
    }

    public void StartSpawning()
    {
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    public void SpawnBoss(GameObject bossPrefab, System.Action onBossDied)
    {
        if (hasSpawnedBoss || bossPrefab == null || player == null) return;

        Vector3 spawnPos = FindValidSpawnPointNearPlayer();

       var boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        if (boss.TryGetComponent(out BaseZombie bossZombie))
        {
            bossZombie.OnDied += onBossDied;
        }
        hasSpawnedBoss = true;
    }
    

    private Vector3 FindValidSpawnPointNearPlayer()
    {
        Vector2 offset = Random.insideUnitCircle.normalized * (spawnRadius * 0.8f);
        Vector3 spawnPos = new Vector3(
            player.position.x + offset.x,
            groundY,
            player.position.z + offset.y
        );
        return spawnPos;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            TrySpawnZombie();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void TrySpawnZombie()
    {
        const int maxAttempts = 10;


        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPos = new Vector3(
                player.position.x + randomCircle.x,
                groundY,
                player.position.z + randomCircle.y
            );

            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(spawnPos);
            if (viewportPoint.x < 0f || viewportPoint.x > 1f || viewportPoint.y < 0f || viewportPoint.y > 1f)
            {
                Instantiate(zombiePrefab, spawnPos, Quaternion.identity, parentSpawn);
                return;
            }
        }

        Debug.LogWarning("No valid zombie spawn position found (outside of camera view).");
    }
}
