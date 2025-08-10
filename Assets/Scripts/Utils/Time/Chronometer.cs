using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chronometer : TimeController
{
    private void Update()
    {
        if (activated)
        {
            currentTime += Time.deltaTime;
            eventTimeModified?.Invoke(currentTime);
        }  
    }

    public override void Reboot()
    {
        Initiate(0);
    }

    public override void End()
    {
        UIManager.instance.SetScoore(currentTime);
        eventEndTime?.Invoke();
        activated = false;
    }
}
