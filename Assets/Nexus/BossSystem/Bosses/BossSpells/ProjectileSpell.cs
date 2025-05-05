using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Spell/Projectile", fileName = "NewProjectileSpell")]
public class ProjectileSpell : BossSpell
{
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float speed = 10f;
    public int count = 1;
    public float spreadAngle = 15f;

    public override void Cast(BossController boss, Transform target)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is not assigned in ProjectileSpell.");
            return;
        }
        if (boss == null || target == null)
        {
            Debug.LogError("Boss or target is not assigned in ProjectileSpell.");
            return;
        }

        if (count > 1)
            SpawnProjectilesSpread(boss, target);
        else
            SpawnProjectile(boss, target);
        
        CastChainSpell(boss, target);
    }

    private void SpawnProjectilesSpread(BossController boss, Transform target)
    {
        Vector3 origin = (spawnPoint != null) ? spawnPoint.position : boss.transform.position;
        Vector3 baseDir = (target.position - origin).normalized;

        for (int i = 0; i < count; i++)
        {
            // rotate baseDir by a random yaw within spreadAngle
            float yaw = Random.Range(-spreadAngle, spreadAngle);
            Vector3 dir = Quaternion.Euler(0, yaw, 0) * baseDir;

            var proj = ObjectPooler.Instance.SpawnFromPool(
                projectilePrefab,
                origin,
                Quaternion.LookRotation(dir)
            );
            if (proj.TryGetComponent<Rigidbody>(out var rb))
                rb.velocity = dir * speed;

            proj.GetComponent<CryomancerFireball>().damage = damage;
            proj.GetComponent<CryomancerFireball>().speed = speed;
        }
    }

    private void SpawnProjectile(BossController boss, Transform target)
    {
        Vector3 origin = (spawnPoint != null) ? spawnPoint.position : boss.transform.position;
        Vector3 dir = (target.position - origin).normalized;

        var proj = ObjectPooler.Instance.SpawnFromPool(
            projectilePrefab,
            origin,
            Quaternion.LookRotation(dir)
        );
        if (proj.TryGetComponent<Rigidbody>(out var rb))
            rb.velocity = dir * speed;
        proj.GetComponent<CryomancerFireball>().damage = damage;
        proj.GetComponent<CryomancerFireball>().speed = speed;
    }
}