using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : TimeController
{
    float initiateTime;
    [SerializeField]
    UnityEvent unityEventEndTime;
    [SerializeField]
    UnityEvent unityEventStartTime;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip startClip;

    public override void Initiate(float time)
    {
        initiateTime = time;
        base.Initiate(time);
    }

    public void Update()
    {
        if (activated)
        {
            currentTime -= Time.deltaTime;
            eventTimeModified?.Invoke(currentTime);

            if (currentTime <= 0)
            {
                End();
            }
        }
    }

    public override void Reboot()
    {
       Initiate(initiateTime);
    }

    public override void End()
    {
        unityEventEndTime?.Invoke();
        eventEndTime?.Invoke();
        //audioSource.PlayOneShot(startClip);
        activated = false;
    }
}
