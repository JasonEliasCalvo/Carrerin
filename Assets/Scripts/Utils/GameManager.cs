using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void DelegatedGameStates();
    public DelegatedGameStates eventGameStart;
    public DelegatedGameStates eventGameEnd;
    public static GameManager instance;

    PlayerControls playerControls;

    [Header("Time Settings")]
    [SerializeField] Timer timer;
    [SerializeField] Chronometer chronometer;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;
    public Image fadeImage;
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;
    [SerializeField] private float fadeTimer = 0;
    
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
        InicializateGame();
    }

    public void InicializateGame()
    {
        playerControls = new PlayerControls();
        fadeImage.color = fadeColor;
        fadeCanvasGroup.alpha = 1f;
        StartFadeIn();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Exit.performed += OnExit;
        playerControls.UI.Continue.performed += OnContinue;
    }

    private void OnContinue(InputAction.CallbackContext context)
    {
        if (UIManager.instance.winPanel.activeSelf)
        {
            ReturnToMainMenu();
        }
    }

    private void OnDisable()
    {
        playerControls.Player.Exit.performed -= OnExit;
        playerControls.UI.Continue.performed -= OnContinue;
        playerControls.Player.Disable();
    }

    public void GamePrepate()
    {
        UIManager.instance.ShowTimerPanel(true);
        timer.eventEndTime += GameStart;
        timer.Initiate(3);
    }

    public void GameStart()
    {
        eventGameStart?.Invoke();
        UIManager.instance.ShowChrometerPanel(true);
        chronometer.Initiate(0);
    }

    public void GameEnd()
    {
        UIManager.instance.winPanel.SetActive(true);
        chronometer.Stop();
        chronometer.End();
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

        fadeTimer = 0;

        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, fadeTimer / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1;
        fadeCanvasGroup.interactable = true;
        fadeCanvasGroup.blocksRaycasts = true;
        onComplete?.Invoke();
    }

    private IEnumerator FadeInCoroutine(System.Action onComplete)
    {
        fadeTimer = 0;

        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, fadeTimer / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.interactable = false;
        fadeCanvasGroup.blocksRaycasts = false;
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

    private void OnExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("Botón de salida presionado");

        if (UIManager.instance.mainPanel.activeSelf)
        {
            Application.Quit();
        }
        if (UIManager.instance.pausePanel.activeSelf)
        {
            TogglePause();
        }
        if (UIManager.instance.creditpanel.activeSelf)
        {
            UIManager.instance.creditpanel.SetActive(false);
            UIManager.instance.mainPanel.SetActive(true);
        }
        if(UIManager.instance.optionsPanel.activeSelf)
        {
            UIManager.instance.optionsPanel.SetActive(false);
            if(Time.timeScale > 0)
            {
                UIManager.instance.mainPanel.SetActive(true);
                Debug.Log("Menu");
            }

            else
                UIManager.instance.pausePanel.SetActive(true);
        }

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
