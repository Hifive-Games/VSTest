using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public EnemyDataSO enemySO;
    public int maxHealth;
    public int damage;
    public float speed;
    public int currentHealth;
    public int Armor;
    public int score;
    public int experience;
    public GameObject expPrefab;
    protected GameObject player;

    // New properties for attack behavior
    protected float attackRange;
    protected float attackSpeed; // cooldown duration
    private float attackTimer = 0f;

    // Keeps track of debuffs
    public List<Debuff> Debuffs { get; private set; } = new List<Debuff>();

    // Caching enemy layer mask.
    private int enemyLayerMask;

    // Fields for movement smoothing.
    public float smoothTime = 0.1f; // adjust as needed for smoothness
    private Vector3 currentMoveDirection = Vector3.zero;
    private Vector3 smoothVelocity = Vector3.zero;

    // Detection settings.
    public float detectionRange = 1.5f; // range for detecting enemies ahead
    public float detectionAngle = 45f; // cone half-angle in degrees
    // Preallocate a collider array for Physics.OverlapSphereNonAlloc.
    private Collider[] overlapResults = new Collider[10];

    public void Initialize(EnemyDataSO enemySO)
    {
        this.enemySO = enemySO;
        maxHealth = enemySO.maxHealth;
        currentHealth = maxHealth;
        damage = enemySO.damage;
        speed = enemySO.speed;
        score = enemySO.score;
        experience = enemySO.experience;
        attackRange = enemySO.attackRange;
        attackSpeed = enemySO.attackSpeed;
        expPrefab = enemySO.deathEffect;
    }

    public virtual void OnEnable()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<CharacterController>().gameObject;
        }
        currentHealth = maxHealth;
        attackTimer = 0f; // reset attack timer on enable

        // Cache the enemy layer mask.
        enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    public virtual void OnDisable()
    {
        EnemySpawner.Instance.RemoveEnemy(gameObject);
        player = null;
        Destroy(this);
    }

    /// <summary>
    /// Update waits for the attack cooldown to finish when within range.
    /// Otherwise, the timer is reset and the enemy moves.
    /// Uses squared distance for optimization.
    /// </summary>
    public virtual void Update()
    {
        if (player == null)
            return;

        Vector3 diff = player.transform.position - transform.position;
        float sqrDistance = diff.sqrMagnitude;
        float attackRangeSqr = attackRange * attackRange;

        if (sqrDistance <= attackRangeSqr)
        {
            // Enemy in attack range—accumulate timer.
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackSpeed)
            {
                AttackPlayer();
                attackTimer = 0f;
            }
        }
        else
        {
            attackTimer = 0f;
            MoveTowardsPlayer();
        }
    }

    private void AttackPlayer()
    {
        Debug.Log($"Enemy {name} tries to attack player(Damage: {damage}).");
        //player.GetComponent<TheHeroDamageManager>().TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        if (hitBySpell)
        {
            Invoke("ResetHitBySpell", 2f);
        }
        else
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ResetHitBySpell()
    {
        Debug.Log("ResetHitBySpell");
        hitBySpell = false;
    }

    public void Die()
    {
        ObjectPooler.Instance.ReturnObject(gameObject);
        Debug.Log("Enemy died");
        Vector3 position = new Vector3(transform.position.x, .5f, transform.position.z);
        GlobalGameEventManager.Instance.Notify("EnemyDied", experience, position, expPrefab);
        Debuffs.Clear();
    }

    public void MoveTowardsPlayer()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 idealDirection = (targetPosition - currentPosition).normalized;
        Vector3 targetDirection = GetAdjustedDirection(idealDirection);

        if (targetDirection == Vector3.zero)
        {
            // If no clear direction, do not move.
            return;
        }

        // Smooth out direction transitions.
        currentMoveDirection = Vector3.SmoothDamp(
            currentMoveDirection,
            targetDirection,
            ref smoothVelocity,
            smoothTime);

        transform.position += currentMoveDirection * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (currentMoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(currentMoveDirection);
        }
    }

    // Adjust the direction based on enemy detection.
    private Vector3 GetAdjustedDirection(Vector3 targetDirection)
    {
        if (IsDirectionClear(targetDirection))
            return targetDirection;

        // Try offsets up to 90 degrees in 15° increments.
        for (int i = 1; i <= 6; i++)
        {
            float angleOffset = 15f * i;
            Vector3 rightDir = Quaternion.Euler(0, angleOffset, 0) * targetDirection;
            if (IsDirectionClear(rightDir))
                return rightDir;

            Vector3 leftDir = Quaternion.Euler(0, -angleOffset, 0) * targetDirection;
            if (IsDirectionClear(leftDir))
                return leftDir;
        }

        return Vector3.zero;
    }

    // Uses a non-allocating overlap sphere; returns false if any enemy lies within the cone.
    private bool IsDirectionClear(Vector3 direction)
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, detectionRange, overlapResults, enemyLayerMask);
        for (int i = 0; i < count; i++)
        {
            Collider hit = overlapResults[i];
            if (hit.gameObject == gameObject)
                continue;

            Vector3 toOther = (hit.transform.position - transform.position).normalized;
            if (Vector3.Angle(direction, toOther) < detectionAngle)
                return false;
        }
        return true;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 forward = transform.forward;
        Vector3 right = Quaternion.Euler(0, detectionAngle, 0) * forward;
        Vector3 left = Quaternion.Euler(0, -detectionAngle, 0) * forward;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, forward * detectionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, right * detectionRange);
        Gizmos.DrawRay(transform.position, left * detectionRange);
    }

    public void ApplyDebuff(Debuff debuff)
    {
        if (!gameObject.activeInHierarchy)
            return;

        switch (debuff.Type)
        {
            case DebuffType.ArmorReduction:
                StartCoroutine(ApplyArmorReduction(debuff));
                break;
                // Add other debuff types as needed.
        }
    }

    public bool HasDebuff(DebuffType debuffType)
    {
        return Debuffs.Exists(d => d.Type == debuffType);
    }

    public void RemoveDebuff(DebuffType debuffType)
    {
        Debuffs.RemoveAll(d => d.Type == debuffType);
    }

    private IEnumerator ApplyArmorReduction(Debuff debuff)
    {
        Debuffs.Add(debuff);
        Armor -= debuff.Value;
        yield return new WaitForSeconds(debuff.Duration);
        Armor += debuff.Value;
        Debuffs.Remove(debuff);
    }

    // Field to help with spell damage handling.
    public bool hitBySpell = false;
}