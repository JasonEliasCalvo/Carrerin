using System.Collections;
using UnityEngine;

public class KartEffectsManager : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    public void LaunchProjectile(KartController owner, bool followTarget)
    {
        Vector3 spawnPos = owner.transform.position + owner.transform.forward * 1.5f;
        Quaternion spawnRot = owner.transform.rotation;

        GameObject projectile = Instantiate(projectilePrefab, spawnPos, spawnRot);

        var projScript = projectile.GetComponent<ProjectileBehavior>();
        projScript.owner = owner;
        projScript.followTarget = followTarget;
    }


    public IEnumerator SpeedBoost(KartController kart, float multiplier, float duration)
    {
        float originalSpeed = kart.forwardSpeed;
        kart.forwardSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        kart.forwardSpeed = originalSpeed;
    }

    public IEnumerator StopEffect(KartController kart, float duration)
    {
        float originalForward = kart.forwardSpeed;
        kart.forwardSpeed = 0.2f;
        kart.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(duration);
        kart.forwardSpeed = originalForward;
    }


    public IEnumerator SlipEffect(KartController kart, float duration)
    {
        float originalDrag = kart.drag;
        kart.drag = 0.2f; // Menos agarre
        yield return new WaitForSeconds(duration);
        kart.drag = originalDrag;
    }
}
