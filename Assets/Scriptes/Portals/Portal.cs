using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private float cooldownDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        var teleportCooldown = collision.GetComponent<TeleportCooldown>();
        if (teleportCooldown == null || !teleportCooldown.canTeleport) return;

        collision.transform.position = teleportTarget.position;
        teleportCooldown.DisableTeleport(cooldownDuration);
    }
}
