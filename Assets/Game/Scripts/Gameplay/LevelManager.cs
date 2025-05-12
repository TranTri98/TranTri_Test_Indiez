using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelData> levels;
    [SerializeField] private Transform levelParent;

    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject winGamePanel;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private TextMeshProUGUI FPSTxt;

    private int currentLevel = 0;
    private float timer;
    private GameObject currentLevelObj;

    private bool bossSpawned = false;
    private bool levelComplete = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
    void Start()
    {
        LoadLevel(0);
    }

    void Update()
    {
        FPSTxt.text = Mathf.FloorToInt((1f / Time.deltaTime)).ToString();
        if (levelComplete) return;

        timer -= Time.deltaTime;
        timeTxt.text = Mathf.FloorToInt(timer).ToString();
        var data = levels[currentLevel];

        if (timer <= 0f)
        {
            timeTxt.text = "0";
            if (data.bossZombiePrefab != null && !bossSpawned)
            {
                SpawnBoss(data.bossZombiePrefab);
                bossSpawned = true;
            }
            else if (data.bossZombiePrefab == null && !levelComplete)
            {
                CompleteLevel();
            }
        }
    }

    void WinGame()
    {
        levelComplete = true;
        winGamePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void CompleteLevel()
    {
        levelComplete = true;
        ZombieSpawner.I.StopSpawning();
        levelCompletePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void SpawnBoss(GameObject bossPrefab)
    {
        ZombieSpawner.I.StopSpawning();
        ZombieSpawner.I.SpawnBoss(bossPrefab, OnBossDied);
    }

    public void OnBossDied()
    {
        if (!levelComplete)
            WinGame();
    }

    void ShowLevelCompletePanel()
    {
        levelCompletePanel.SetActive(true);

        Time.timeScale = 0f; // pause game
    }

    void LoadLevel(int index)
    {
        foreach (var zombie in GameObject.FindGameObjectsWithTag("Zombie"))
            Destroy(zombie);
        if (currentLevelObj != null)
            Destroy(currentLevelObj);
        levelComplete = false;
        playerHealth.transform.position = new Vector3(0, 1, 0);

        playerHealth.ResetHealth();

        var data = levels[index];
        timer = data.duration;
        currentLevelObj = Instantiate(data.levelPrefab, levelParent);

        var surface = currentLevelObj.GetComponent<NavMeshSurface>();
        
        if (surface != null)
        {
            StartCoroutine(BuildNavMeshNextFrame(surface));
        }

        ZombieSpawner.I.SetSpawnInterval(data.spawnInterval);
        ZombieSpawner.I.StartSpawning();

    }

    IEnumerator BuildNavMeshNextFrame(NavMeshSurface surface)
    {
        yield return null;
        surface.BuildNavMesh();
    }

    void NextLevel()
    {
        currentLevel++;
        if (currentLevel < levels.Count)
            LoadLevel(currentLevel);
        else
            UIManager.I.ShowCompletePanel();
    }

    public void OnClickNextLevel()
    {
        Time.timeScale = 1f;
        levelCompletePanel.SetActive(false);
        NextLevel();
    }

    public void OnClickResetLevel()
    {
        Time.timeScale = 1f;
        levelCompletePanel.SetActive(false);

        playerHealth.ResetHealth(); 

        foreach (var zombie in GameObject.FindGameObjectsWithTag("Zombie"))
            Destroy(zombie);

        LoadLevel(currentLevel); 
    }

    public void OnQuitApplication()
    {
        Application.Quit();
    }
}
