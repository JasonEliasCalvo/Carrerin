using System.Collections;
using UnityEngine;

public class KartEffectsManager : MonoBehaviour
{
    private GameObject projectilePrefab;

    public void LaunchProjectile(KartController owner, bool followTarget)
    {
        GameObject projectile = Instantiate(projectilePrefab, owner.transform.position + owner.transform.forward, Quaternion.identity);

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
