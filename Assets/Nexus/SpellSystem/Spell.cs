using System;
using System.Collections;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public int SpellID;
    public string Name;
    public string Description;
    public Sprite Icon;
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

    public Caster Caster;

    public virtual void OnEnable()
    {
        target = null;
    }

    public virtual void OnDisable()
    {
        target = null;
    }

    public virtual Transform FindClosestEnemy()
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] hits = Physics.OverlapSphere(transform.position, range, mask);

        Transform closest = null;
        float closestDistance = range;
        foreach (var hit in hits)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = hit.transform;
            }
        }

        return closest;
    }

    public virtual void Seek(Transform target = null)
    {
        if (target == null)
        {
            this.target = FindClosestEnemy();
        }
        else
        {
            this.target = target;
        }
    }



    public virtual void Release()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            target = FindClosestEnemy();
        }
        StartCoroutine(MoveTowardsTarget());
    }



    public IEnumerator MoveTowardsTarget()
    {

        float radius = 1.5f; // Orbit radius around caster
        float angle = 0f;
        while (duration > 0)
        {
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                Transform casterTransform = TheHero.Instance.transform; // Assuming TheHero.Instance is the caster's transform
                if (casterTransform == null)
                    yield break;

                angle += Time.deltaTime * 20; // Rotate speed

                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                Vector3 offset = new Vector3(x, 0, z);
                transform.position = casterTransform.position + offset;

                duration -= Time.deltaTime;
                Seek();
                yield return null;
            }
            else
            {
                Vector3 direction = target.position - transform.position;
                float distanceThisFrame = speed * Time.deltaTime;

                if (direction.magnitude <= distanceThisFrame)
                {
                    print("Hit target");
                    yield break;
                }

                transform.Translate(direction.normalized * distanceThisFrame, Space.World);
                transform.LookAt(transform.position + direction);
            }

            duration -= Time.deltaTime;
            yield return null;
        }

        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy) && Caster == Caster.Player)
        {
            CollisionEffect(enemy);

            print($"Cast by {Caster} collided with {enemy.name}");
        }

        if (other.TryGetComponent(out CharacterController player) && Caster == Caster.Boss)
        {
            CollisionEffect(player);

            print($"Cast by {Caster} collided with {player.name}");
        }
    }

    public virtual void CollisionEffect(Enemy enemy = null)
    {
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    public virtual void CollisionEffect(CharacterController player)
    {
        ObjectPooler.Instance.ReturnObject(gameObject);
    }
}

