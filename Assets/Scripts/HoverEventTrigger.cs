using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoverEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Eventos que se ejecutan al pasar el rat�n")]
    public UnityEvent onHover;

    [Header("Eventos que se ejecutan cuando el rat�n sale")]
    public UnityEvent outHover;

    public void OnDeselect(BaseEventData eventData)
    {
        outHover?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outHover?.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {
        onHover?.Invoke();
    }
}
