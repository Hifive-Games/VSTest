using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public EnemyDataSO enemySO;
    public int maxHealth;
    public int damage = 10;
    public float speed;
    public int currentHealth;
    public int Armor;
    public int score;
    public int experience;
    public GameObject expPrefab;
    protected GameObject player;
    public GameObject Player => player;

    // New properties for attack behavior
    protected float attackRange;
    protected float attackSpeed;
    protected float attackCooldown;
    private float attackTimer = 0f;

    // Keeps track of debuffs
    public List<Debuff> Debuffs { get; private set; } = new List<Debuff>();

    // Caching enemy layer mask.
    [SerializeField] LayerMask enemyLayerMask;

    // Fields for movement smoothing.
    public float smoothTime = 0.1f; // adjust as needed for smoothness
    private Vector3 currentMoveDirection = Vector3.zero;
    private Vector3 smoothVelocity = Vector3.zero;

    // Detection settings.
    public float detectionRange = 1.5f; // range for detecting enemies ahead
    public float detectionAngle = 45f; // cone half-angle in degrees
    // Preallocate a collider array for Physics.OverlapSphereNonAlloc.
    private Collider[] overlapResults = new Collider[10];

    public float physicsCheckInterval = 0.2f; // update physics check every 0.2 seconds
    private float physicsCheckTimer = 0f;
    private Vector3 cachedDirection = Vector3.zero;

    private GameObject attackPrefab;

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
        attackCooldown = enemySO.attackCooldown;
        attackPrefab = enemySO.enemyAttackPrefab;
        expPrefab = enemySO.deathEffect;

        //int currentLevel = TheHeroExperienceManager.Instance.GetCurrentLevel();

        int currentLevel = TheHero.Instance.GetComponent<TheHeroExperienceManager>().GetCurrentLevel();
        
        {
            maxHealth += Mathf.FloorToInt(maxHealth * (currentLevel / 30f));
            currentHealth = maxHealth;

            damage += Mathf.FloorToInt(damage * (currentLevel / 50f));
        }
    }

    private void Start()
    {
        player = TheHero.Instance.gameObject;
        if (player == null)
        {
            Debug.LogError("Player not found.");
            return;
        }
    }

    public virtual void OnEnable()
    {
        attackTimer = 0f;

        // Cache the enemy layer mask.(enemy and obstacle layers)
        enemyLayerMask = LayerMask.GetMask("Enemy", "Obstacle");

    }

    public virtual void OnDisable()
    {
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

        float sqrDistance = (player.transform.position - transform.position).sqrMagnitude;
        float attackRangeSqr = attackRange * attackRange;


        attackTimer -= Time.deltaTime;

        if (sqrDistance <= attackRangeSqr)
        {
            // countdown
            if (attackTimer <= 0f)
            {
                AttackPlayer();
                attackTimer = attackCooldown + attackSpeed;
            }
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    public void MoveTowardsPlayer()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 idealDirection = (targetPosition - currentPosition).normalized;

        // Update the cached adjusted direction only at set intervals.
        physicsCheckTimer += Time.deltaTime;
        if (physicsCheckTimer >= physicsCheckInterval)
        {
            cachedDirection = GetAdjustedDirection(idealDirection);
            physicsCheckTimer = 0f;
        }

        // Early return if there's no clear direction.
        if (cachedDirection == Vector3.zero)
        {
            return;
        }

        // Smooth out direction transitions.
        currentMoveDirection = Vector3.SmoothDamp(
            currentMoveDirection,
            cachedDirection,
            ref smoothVelocity,
            smoothTime);

        transform.position += currentMoveDirection * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (currentMoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(currentMoveDirection);
        }
    }

    private void AttackPlayer()
    {
        //mid point between enemy and player
        Vector3 midPoint = (player.transform.position + transform.position) / 2;
        midPoint.y = .1f;
        var go = ObjectPooler.Instance.SpawnFromPool(enemySO.enemyAttackPrefab, midPoint, Quaternion.identity);
        var atk = go.GetComponent<EnemyAttack>();
        atk.SetAttackData(damage, attackRange, attackSpeed);
    }

    public virtual void TakeDamage(int damage)
    {
        if (hitBySpell)
        {
            StartCoroutine(ResetHitBySpell());
        }

        currentHealth -= damage;
        if (player != null)
            SFXManager.Instance.PlayAt(SFX.EnemyHit);

        DamageNumberManager.Instance.ShowDamage(damage, transform.position);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public virtual void TakeDamage(int damage, DamageNumberType damageNumberType)
    {
        if (hitBySpell)
        {
            StartCoroutine(ResetHitBySpell());
        }

        currentHealth -= damage;
        if (player != null)
            SFXManager.Instance.PlayAt(SFX.EnemyHit);
        DamageNumberManager.Instance.ShowDamage(damage, transform.position, damageNumberType);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator ResetHitBySpell()
    {
        yield return new WaitForSeconds(2f);
        hitBySpell = false;
    }


    public virtual void Die()
    {
        SFXManager.Instance.PlayAt(SFX.EnemyDie);

        EnemySpawner.Instance.RemoveEnemy(gameObject);
        ObjectPooler.Instance.ReturnObject(gameObject);

        Vector3 position = new Vector3(transform.position.x, .5f, transform.position.z);
        GlobalGameEventManager.Instance.Notify("EnemyDied", experience, position, expPrefab);

        Debuffs.Clear();
        EnemySpawner.Instance.AddKill();
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