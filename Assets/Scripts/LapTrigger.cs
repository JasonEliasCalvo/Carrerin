using System.Collections.Generic;
using UnityEngine;

public class LapTrigger : MonoBehaviour
{
    public int totalLaps = 3;

    // Guardamos vueltas por jugador
    private Dictionary<GameObject, int> playerLaps = new Dictionary<GameObject, int>();

    private void OnTriggerEnter(Collider other)
    {
        // Verifica que sea un jugador (puedes cambiar la etiqueta)
        if (!other.CompareTag("Player"))
            return;

        GameObject player = other.gameObject;

        // Si es la primera vez que pasa, lo agregamos
        if (!playerLaps.ContainsKey(player))
        {
            playerLaps[player] = 0;
        }

        // Sumamos una vuelta
        playerLaps[player]++;

        Debug.Log($"{player.name} completó {playerLaps[player]} vueltas");

        // Si llegó al número de vueltas, avisamos al GameManager
        if (playerLaps[player] >= totalLaps)
        {
            GameManager.instance.GameEnd();
        }
    }
}
