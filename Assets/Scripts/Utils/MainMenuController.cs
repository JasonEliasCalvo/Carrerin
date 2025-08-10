using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public void PlayButtonC(MonoBehaviour playerInput)
    {
        gameObject.GetComponentInChildren<Button>().enabled = false;
        GameManager.instance.StartFadeOut(() => {
            playerInput.enabled = true;

            UIManager.instance.ShowSelectedPanel(true);
            GameManager.instance.StartFadeIn(() =>
            {
                gameObject.GetComponentInChildren<Button>().enabled = true;
                gameObject.SetActive(false);
            });
        });
    }

    public void QuitButton()
    {
        Debug.Log("Salir del juego...");
        Application.Quit();
    }
}
