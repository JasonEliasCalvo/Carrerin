using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSpawnScript : MonoBehaviour
{
    public Transform[] SpawnPoints;
    private int m_playerCount;
    public Image[] playerStatusTexts;
    public GameObject loading;
    public Sprite confirmText;
    public Sprite pressKeyText;

    private bool loadingStarted = false;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log(playerInput.name);
        playerInput.transform.position = SpawnPoints[m_playerCount].transform.position;
        playerInput.transform.rotation = SpawnPoints[m_playerCount].transform.rotation;

        var cam = playerInput.GetComponent<CameraDeviceAssigner>();
        var cam_ctrl = cam.inputProvider;
        cam_ctrl.PlayerIndex = playerInput.playerIndex;

        var modelSelector = playerInput.GetComponent<CharacterModelSelector>();
        modelSelector.SetCharacterModel(m_playerCount);

        PlayerConfirmed(m_playerCount);

        m_playerCount++;
    }

    public void PlayerConfirmed(int playerIndex)
    {
        if (loadingStarted) return;

        playerStatusTexts[playerIndex].sprite = confirmText;

        if (playerIndex == 1)
        {
            StartCoroutine(StartFakeLoading());
        }
    }

    private IEnumerator StartFakeLoading()
    {
        loadingStarted = true;
        loading.GetComponent<Animator>().enabled = true;

        yield return new WaitForSeconds(3f);

        GameManager.instance.StartFadeOut(() => {
            UIManager.instance.ShowSelectedPanel(false);
            GameManager.instance.StartFadeIn(() =>
            {
                GameManager.instance.GamePrepate();
            });  
        });
    }
}


