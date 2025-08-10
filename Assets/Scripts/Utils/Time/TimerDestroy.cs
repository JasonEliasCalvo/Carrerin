using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerDestroy : MonoBehaviour
{
    [SerializeField]
    private float time;

    public void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Destroy(gameObject);
        }
    }
}
