using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraDeviceAssigner : MonoBehaviour
{
    public CinemachineInputAxisController inputProvider;
    public PlayerInput playerInput;

    void OnEnable()
    {
        GameManager.instance.eventGameStart += ActivateCamera;
        GameManager.instance.eventGameEnd += DeactivateCamera;
    }

    void OnDisable()
    {
        GameManager.instance.eventGameStart -= ActivateCamera;
        GameManager.instance.eventGameEnd -= DeactivateCamera;
    }

    void ActivateCamera()
    {
        inputProvider.enabled = true;
    }

    void DeactivateCamera()
    {
        inputProvider.enabled = false;
    }

}
