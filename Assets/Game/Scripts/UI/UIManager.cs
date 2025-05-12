using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager I { get; private set; }

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverMask;   // black UI panel that blocks input
    [SerializeField] private CanvasGroup fadeCanvasGroup; // for fade-in effect
    [SerializeField] private float fadeDuration = 0.5f;

    void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);

        if (gameOverMask != null)
        {
            gameOverMask.SetActive(false); // Hide at start
        }

    }
    public void ShowCompletePanel()
    {

        if (fadeCanvasGroup != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    public void ShowGameOver()
    {
        if (gameOverMask != null)
        {
            gameOverMask.SetActive(true); // Enable Raycast Target to block joystick
        }

        if (fadeCanvasGroup != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }
}
