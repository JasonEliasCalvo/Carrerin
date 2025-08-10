using UnityEngine;
using UnityEngine.EventSystems;

public class AlwaysSelected÷bject : MonoBehaviour
{
    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }
}