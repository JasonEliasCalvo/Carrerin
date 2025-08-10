using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAutoSelector : MonoBehaviour
{
    [Tooltip("Bot�n por defecto al abrir el panel")]
    public GameObject defaultButton;

    private void Update()
    {
        var current = EventSystem.current.currentSelectedGameObject;

        // Si no hay nada seleccionado o el actual est� inactivo
        if (current == null || !current.activeInHierarchy)
        {
            TrySelectValidButton();
        }
    }

    void TrySelectValidButton()
    {
        // Intenta seleccionar el bot�n por defecto si est� activo
        if (defaultButton != null && defaultButton.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
            return;
        }

        // Si no hay default o est� desactivado, busca el primer bot�n activo en el panel
        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (var btn in buttons)
        {
            if (btn.gameObject.activeInHierarchy && btn.interactable)
            {
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
                return;
            }
        }
    }
}
