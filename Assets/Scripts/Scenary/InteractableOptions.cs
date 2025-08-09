using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InteractionType
{
    InvokeEvent,
    Upgrade,
    Degrade,
    Use
}

public enum ItemType
{
    Projectile,
    SpeedBoost,
    Trap
}

public class InteractableOptions : MonoBehaviour
{
    [SerializeField] private List<InteractionType> interactionTypes = new List<InteractionType>();
    [SerializeField] private ItemType itemType;
    [SerializeField] private bool justOneInteraction = false;
    [SerializeField] private bool canInteract = true;
    [SerializeField] public bool useOnPickup = true;

    public List<UnityEvent> onInteract = new List<UnityEvent>();

    [Header("Configuración de Interacción")]
    public KartEffectsManager effectsManager;
    public string materialName;
    public int ID;

    public List<InteractionType> InteractionTypes => interactionTypes;

    private void Start()
    {
        effectsManager = FindFirstObjectByType<KartEffectsManager>();
    }

#if UNITY_EDITOR
    private void Reset()
    {
        EnsureDefaults();
    }

    private void OnValidate()
    {
        EnsureDefaults();
    }

    private void EnsureDefaults()
    {
        if (interactionTypes == null || interactionTypes.Count == 0)
        {
            interactionTypes = new List<InteractionType> { InteractionType.InvokeEvent };
        }

        if (onInteract == null || onInteract.Count == 0)
        {
            onInteract = new List<UnityEvent> { new UnityEvent() };
        }
    }
#endif

    public void TryInteract(KartController kart)
    {
        if (!canInteract) return;

        if (justOneInteraction)
        {
            canInteract = false;
            ExecuteInteraction(kart);
        }
        else
        {
            ExecuteInteraction(kart);
        }
    }

    private void ExecuteInteraction(KartController kart)
    {
        foreach (var type in interactionTypes)
        {
            switch (type)
            {
                case InteractionType.InvokeEvent:
                    foreach (UnityEvent evt in onInteract)
                        evt?.Invoke();
                    break;

                case InteractionType.Upgrade:
                    if (effectsManager != null)
                        StartCoroutine(effectsManager.SpeedBoost(kart, 2f, 3f));
                    break;

                case InteractionType.Degrade:
                    if (effectsManager != null)
                        StartCoroutine(effectsManager.StopEffect(kart, 1.5f));
                    break;

                case InteractionType.Use:
                    Use(kart);
                    break;
            }
        }
    }

    public void Use(KartController owner)
    {
        if (effectsManager == null) return;

        switch (itemType)
        {
            case ItemType.Projectile:
                effectsManager.LaunchProjectile(owner, true);
                break;
            case ItemType.SpeedBoost:
                StartCoroutine(effectsManager.SpeedBoost(owner, 2f, 3f));
                break;
            case ItemType.Trap:
                // Lógica para colocar trampa
                break;
        }
    }

    public void EnableInteraction() => canInteract = true;
    public void DisableInteraction() => canInteract = false;
}
