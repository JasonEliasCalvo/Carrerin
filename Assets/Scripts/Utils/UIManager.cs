using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();

    [Header("UI Elements")]
    public GameObject optionsPanel;
    public GameObject pausePanel;
    public GameObject confirmPanel;
    public GameObject warningPanel;
    public GameObject inventoryPanel;
    public TextMeshProUGUI confirmText;
    public TextMeshProUGUI warningText;

    [Header("UI Sounds")]
    public AudioClip sound;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (IsPanelActive())
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public bool IsPanelActive()
    {
        return
      (pausePanel != null && pausePanel.activeSelf) ||
      (optionsPanel != null && optionsPanel.activeSelf) ||
      (confirmPanel != null && confirmPanel.activeSelf) ||
      (warningPanel != null && warningPanel.activeSelf);
    }

    public void AnimatePanelIn(GameObject panel)
    {
        if (panel == null) return;
        RectTransform rt = panel.GetComponent<RectTransform>();

        if (!originalScales.ContainsKey(panel))
        {
            originalScales[panel] = rt.localScale;
        }

        rt.localScale = Vector3.zero;

        panel.SetActive(true);

        rt.DOScale(originalScales[panel], 0.3f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
    }

    public void AnimatePanelOut(GameObject panel)
    {
        if (panel == null) return;
        RectTransform rt = panel.GetComponent<RectTransform>();

        rt.DOScale(Vector3.zero, 0.25f)
          .SetEase(Ease.InBack)
          .SetUpdate(true)
          .OnComplete(() =>
          {
              panel.SetActive(false);
              panel.transform.localScale = Vector3.zero;
          });
    }

    public void ShowPausePanel(bool state)
    {
        pausePanel.SetActive(state);
    }

    public void ShowConfirmPanel(bool state, string message = "")
    {
        confirmText.text = message;
        if (state)
        {
            AnimatePanelIn(confirmPanel);
        }
        else
        {
            AnimatePanelOut(confirmPanel);
        }
    }

    public void ShowWarningPanel(bool state, string message = "")
    {
        warningText.text = message;
        if (state)
        {
            AnimatePanelIn(warningPanel);
        }
        else
        {
            AnimatePanelOut(warningPanel);
        }
    }

    public void ShowOptionsPanel(bool state)
    {
        optionsPanel.SetActive(state);
    }

    public void ShowCursor(bool state)
    {
        if (state)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
