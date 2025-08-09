using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnScript : MonoBehaviour
{
    public Transform[] SpawnPoints;
    private int m_playerCount;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log(playerInput.name);
        playerInput.transform.position = SpawnPoints[m_playerCount].transform.position;

        var cam = playerInput.GetComponent<CameraDeviceAssigner>();
        var cam_ctrl = cam.inputProvider;
        cam_ctrl.PlayerIndex = playerInput.playerIndex;

        m_playerCount++;
    }
}


