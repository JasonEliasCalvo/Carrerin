using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void DelegatedGameStates();
    public DelegatedGameStates eventGameStart;
    public DelegatedGameStates eventGameEnd;
    public static GameManager instance;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;
    public Image fadeImage;
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;
    [SerializeField] private float timer = 0;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        GamePrepate();
    }

    public void GamePrepate()
    {
        fadeImage.color = fadeColor;
        fadeCanvasGroup.alpha = 1f;
        StartFadeIn();
        Invoke(nameof(InitialGameStart), 0.2f);
    }

    public void InitialGameStart()
    {
        eventGameStart?.Invoke();
    }

    public void InitialGameEnd()
    {
        eventGameEnd?.Invoke();
    }

    public void StartFadeOut(System.Action onComplete = null)
    {
        StartCoroutine(FadeOutCoroutine(onComplete));
    }

    public void StartFadeIn(System.Action onComplete = null)
    {
        StartCoroutine(FadeInCoroutine(onComplete));
    }

    private IEnumerator FadeOutCoroutine(System.Action onComplete)
    {
        if (fadeImage != null)
        {
            fadeImage.color = fadeColor;
        }

        timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1;
        onComplete?.Invoke();
    }

    private IEnumerator FadeInCoroutine(System.Action onComplete)
    {
        timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0;
        onComplete?.Invoke();
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(FadeAndLoad(sceneIndex));
        Time.timeScale = 1;
        UIManager.instance.ShowCursor(true);
    }

    public IEnumerator FadeAndLoad(int sceneIndex)
    {
        yield return StartCoroutine(FadeOutCoroutine(null));
        SceneManager.LoadScene(sceneIndex);
    }


    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void ResumeGame()
    {
        UIManager.instance.ShowPausePanel(false);
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void TogglePause()
    {
        if (!UIManager.instance.pausePanel.activeSelf)
        {
            UIManager.instance.ShowPausePanel(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1f;
        }
        else
        {
            UIManager.instance.ShowPausePanel(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }
}
