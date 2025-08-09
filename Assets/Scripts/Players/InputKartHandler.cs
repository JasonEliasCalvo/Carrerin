using UnityEngine;
using UnityEngine.InputSystem;

public class InputKartHandler : MonoBehaviour
{
    private KartController kart;

    private void Awake()
    {
        kart = GetComponent<KartController>();
    }

    public void OnUse(InputValue value)
    {
        if (value.isPressed)
        {
            kart.UseItem();
        }
    }

    public void OnPause(InputValue value)
    {
        if (value.isPressed)
        {
            GameManager.instance.TogglePause();
        }
    }
}
