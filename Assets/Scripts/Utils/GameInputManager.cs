using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour
{
    private PlayerControls _controls;
    public static event Action OnInteractPressed;

    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        _controls.UI.Enable();
        _controls.UI.Pause.performed += OnPauseInput;
        _controls.UI.Interact.performed += OnInteractInput;
    }

    private void OnDisable()
    {
        _controls.UI.Pause.performed -= OnPauseInput;
        _controls.UI.Interact.performed -= OnInteractInput;
        _controls.UI.Disable();
    }

    private void OnPauseInput(InputAction.CallbackContext ctx)
    {
        if (UIManager.instance.optionsPanel.activeSelf)
        {
            UIManager.instance.optionsPanel.SetActive(false);
            UIManager.instance.pausePanel.SetActive(true);
            return;
        }

        GameManager.instance.TogglePause();
    }

    private void OnInteractInput(InputAction.CallbackContext ctx)
    {
        OnInteractPressed?.Invoke();
    }
}
