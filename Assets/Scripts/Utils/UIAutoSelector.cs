using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAutoSelector : MonoBehaviour
{
    [Tooltip("Botón por defecto al abrir el panel")]
    public GameObject defaultButton;

    private void Update()
    {
        var current = EventSystem.current.currentSelectedGameObject;

        // Si no hay nada seleccionado o el actual está inactivo
        if (current == null || !current.activeInHierarchy)
        {
            TrySelectValidButton();
        }
    }

    void TrySelectValidButton()
    {
        // Intenta seleccionar el botón por defecto si está activo
        if (defaultButton != null && defaultButton.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
            return;
        }

        // Si no hay default o está desactivado, busca el primer botón activo en el panel
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
