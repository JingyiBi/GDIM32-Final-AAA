using UnityEngine;

public static class InteractionDetection
{
    public static bool IsPlayerLookingAtTarget(Transform player, Transform target, float maxDistance, float sightAngle = 0.9f)
    {
        float distance = Vector3.Distance(player.position, target.position);
        if (distance > maxDistance) return false;

        Vector3 directionToTarget = (target.position - player.position).normalized;
        float dot = Vector3.Dot(player.forward, directionToTarget);
        return dot > sightAngle;
    }
}