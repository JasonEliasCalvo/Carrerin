using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public string interactionTag;

    [Space(20)]

    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerStay;
    public UnityEvent onTriggerExit;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(interactionTag))
        {
            var kart = other.GetComponent<KartController>();
            if (kart != null)
            {
                var pickup = GetComponent<InteractableOptions>();
                if (pickup != null)
                {
                    if (pickup.useOnPickup)
                    {
                        pickup.Use(kart);
                    }
                    else
                    {
                        kart.PickupItem(pickup);
                    }
                }
            }
            onTriggerEnter.Invoke();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(interactionTag))
        {
            onTriggerExit.Invoke();
        }
    }
}
