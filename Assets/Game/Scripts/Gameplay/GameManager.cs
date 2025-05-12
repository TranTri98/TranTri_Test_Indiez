using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }
    public bool IsGameOver { get; private set; }

    void Awake() => I = this;

    public void SetGameOver()
    {
        IsGameOver = true;
        Time.timeScale = 0f;          // optional: freeze world entirely
    }
}
