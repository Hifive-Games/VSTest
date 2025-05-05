using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Spell/Teleport", fileName = "NewTeleportSpell")]
public class TeleportSpell : BossSpell
{
    private bool preserveY = true;

    public override void Cast(BossController boss, Transform target)
    {
        if (boss == null)
        {
            Debug.LogError("Boss is not assigned in TeleportSpell.");
            return;
        }

        Teleport(boss, target);
        CastChainSpell(boss, target);
    }

    private void Teleport(BossController boss, Transform target, int attempt = 0)
    {
        if (boss == null)
        {
            Debug.LogError("Boss is not assigned in TeleportSpell.");
            return;
        }

        if (attempt > 10)
        {
            Debug.LogError("Too many failed teleport attempts, aborting.");
            return;
        }

        // centre on the player
        Vector3 center = target.position;
        Vector2 offset2D = Random.insideUnitCircle * range;

        float yPos = preserveY
            ? boss.transform.position.y
            : center.y + offset2D.y;

        Vector3 candidate = new Vector3(
            center.x + offset2D.x,
            yPos,
            center.z + offset2D.y
        );

        // if it collides, retry
        if (Physics.CheckSphere(candidate, 0.5f))
        {
            Debug.Log("Teleport spot invalid, retrying...");
            Teleport(boss, target, attempt + 1);
            return;
        }

        boss.transform.position = candidate;
    }
}