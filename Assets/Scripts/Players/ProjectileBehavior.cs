using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [HideInInspector] public KartController owner;
    [HideInInspector] public bool followTarget = false;

    public float speed = 15f;
    public float turnSpeed = 2f;
    public float maxFollowTime = 3f;

    private float followTimer;
    private Transform target;

    private void Start()
    {
        if (followTarget)
        {
            target = FindClosestEnemy();
            followTimer = maxFollowTime;
        }
    }

    private void Update()
    {
        if (followTarget && target != null && followTimer > 0)
        {
            // Moverse hacia el objetivo con rotación suave
            Vector3 dir = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
            followTimer -= Time.deltaTime;
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private Transform FindClosestEnemy()
    {
        KartController[] allKarts = FindObjectsByType<KartController>(FindObjectsSortMode.None);
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (KartController kart in allKarts)
        {
            if (kart == owner) continue; // no seguir al que lo lanzó

            float dist = Vector3.Distance(transform.position, kart.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = kart.transform;
            }
        }
        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        KartController hitKart = other.GetComponent<KartController>();
        if (hitKart != null && hitKart != owner)
        {
           var effectsManager = FindFirstObjectByType<KartEffectsManager>();
            if (effectsManager != null)
                StartCoroutine(effectsManager.StopEffect(hitKart, 1.5f));

            gameObject.GetComponentInChildren<Renderer>().enabled = true;
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
