using UnityEngine;
using TMPro;

public class TimeControllerText : MonoBehaviour
{
    TextMeshProUGUI textMeshPro;
    [SerializeField]
    TimeController time;
    [SerializeField]
    private string formato;
    [SerializeField]
    private GameObject firework;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        time.eventTimeModified += UpdateTimetext;
    }

    public void UpdateTimetext(float newTime)
    {
        textMeshPro.text = newTime.ToString(formato);
    }
}
