using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyDataSO enemySO;
    public int maxHealth;
    public int damage;
    public float speed;
    public int currentHealth;
    public int score;
    public int experience;
    public bool hitBySpell = false;
    public GameObject expPrefab;
    protected GameObject player;

    // New properties
    protected float attackRange;
    protected float attackSpeed;

    protected bool isPlayerInRange;
    private int randomFactor;

    public void Initialize(EnemyDataSO enemySO)
    {
        maxHealth = enemySO.maxHealth;
        currentHealth = maxHealth;
        damage = enemySO.damage;
        speed = enemySO.speed;
        score = enemySO.score;
        experience = enemySO.experience;
        attackRange = enemySO.attackRange;
        attackSpeed = enemySO.attackSpeed;
    }

    protected void OnEnable()
    {
        if (player == null)
        {
            player = Player.Instance.gameObject;
        }

        randomFactor = RandomFactor();

        currentHealth = maxHealth;
    }

    protected void OnDisable()
    {
        EnemySpawner.Instance.RemoveEnemy(gameObject);
        player = null;
        Destroy(this);
    }

    private void ResetHitBySpell()
    {
        print("ResetHitBySpell");
        hitBySpell = false;
    }

    public void Update()
    {
        HostileAction();
    }
    public void TakeDamage(int damage)
    {
        if (hitBySpell)
        {
            Invoke("ResetHitBySpell", 2f);
        }
        else
            currentHealth -= damage;


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        ObjectPooler.Instance.ReturnObject(gameObject);

        Debug.Log("Enemy died");
    }

    public void TryToHitPlayer()
    {
        if (Time.time > attackSpeed)
        {
            Player.Instance.TakeDamage(damage);
            attackSpeed = Time.time + 1f;

            isPlayerInRange = false;
        }
    }

    public void MoveTowardsPlayer()
    {
        if (isPlayerInRange)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        Vector3 direction = player.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void IsPlayerInRange()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < attackRange + randomFactor)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    private int RandomFactor()
    {
        return Random.Range(-1, 1);
    }

    public void HostileAction()
    {
        if (isPlayerInRange)
        {
            TryToHitPlayer();
        }
        else
        {
            IsPlayerInRange();
            MoveTowardsPlayer();
        }
    }
}
