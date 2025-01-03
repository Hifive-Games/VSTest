using System.Collections;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public int SpellID;
    public string Name;
    public string Description;
    public Sprite Icon;
    public int level = 1;
    public float speed = 10f;
    public int damage = 50;
    public float duration = 2f;
    public float range = 10f;
    public float cooldown = 5f;
    public Transform target { get; set; }
    public float explosionRadius = 5f;
    public int projectileCount = 1;
    public float radius = 3f;
    public float tickInterval = 0.5f;

    public virtual void OnEnable()
    {
        Seek(FindClosestEnemy());
        Release();
    }

    public virtual void OnDisable()
    {
        target = null;
    }

    public virtual Transform FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float closestDistance = Mathf.Min(range, float.MaxValue);
        GameObject closestEnemy = null;
        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.gameObject;
            }
        }

        if (closestEnemy != null)
        {
            return closestEnemy.transform;
        }
        else
        {
            return null;
        }
    }

    public void Seek(Transform target)
    {
        this.target = target;
    }



    public virtual void Release()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            FindClosestEnemy();
        }
        StartCoroutine(MoveTowardsTarget());
    }



    public IEnumerator MoveTowardsTarget()
    {
        if (target == null)
        {
            float _duration = duration;
            Vector3 randomDirection;
            randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            transform.LookAt(transform.position + randomDirection);
            while (_duration > 0)
            {
                transform.Translate(randomDirection.normalized * speed * Time.deltaTime, Space.World);
                _duration -= Time.deltaTime;
                yield return null;
            }

            CollisionEffect(null);
        }
        else
        {
            while (target != null)
            {
                Vector3 direction = target.position - transform.position;
                float distanceThisFrame = speed * Time.deltaTime;

                if (direction.magnitude <= distanceThisFrame)
                {
                    CollisionEffect(target.GetComponent<Enemy>());
                    break;
                }

                transform.Translate(direction.normalized * distanceThisFrame, Space.World);
                transform.LookAt(transform.position + direction);
                yield return null;
            }
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            CollisionEffect(enemy);
        }
    }

    public virtual void CollisionEffect(Enemy enemy)
    {
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    public void Upgrade(Upgrade upgrade)
    {
        switch (upgrade.Target)
        {
            case UpgradeTarget.Damage:
                damage += (int)upgrade.GetValue(level);
                break;
            case UpgradeTarget.Cooldown:
                cooldown -= upgrade.GetValue(level);
                break;
            case UpgradeTarget.Range:
                range += upgrade.GetValue(level);
                break;
            case UpgradeTarget.Duration:
                duration += upgrade.GetValue(level);
                break;
            case UpgradeTarget.Speed:
                speed += upgrade.GetValue(level);
                break;
            case UpgradeTarget.Radius:
                radius += upgrade.GetValue(level);
                break;
            case UpgradeTarget.TickInterval:
                tickInterval += upgrade.GetValue(level);
                break;
        }
    }
}
