using UnityEngine;

public class TeleportCooldown : MonoBehaviour
{
    public bool canTeleport = true;

    public void DisableTeleport(float duration)
    {
        StartCoroutine(TeleportDelay(duration));
    }

    private System.Collections.IEnumerator TeleportDelay(float duration)
    {
        canTeleport = false;
        yield return new WaitForSeconds(duration);
        canTeleport = true;
    }
}
