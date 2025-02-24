using UnityEngine;

public class ExampleBoss : Enemy
{

    // Todo: combine enemy and boss classes and make proper Boss class (work in progress)
    public static ExampleBoss Instance;
    private BossSystem.BossController _boss;

    public float actionTimer = 5f;

    public Transform randomPoint;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnEnable()
    {
        Initialize(enemySO);
    }

    public override void OnDisable()
    {
        
    }

    void Start()
    {
        // Create a new boss with initial Spawning state and
        // a BeforeSpell Phase (for "The Slasher" boss, use BeforeSpellSlasherPhase)
        _boss = new BossSystem.BossController(
            new BossSystem.SpawningState(),
            new BeforeSpellSlasherPhase()
        );

        // Set boss HP for GUI
        currentHealth = maxHealth;

    }

    public override void Update()
    {
        // Each frame, update both the current State and Phase
        _boss.Update();

        // Update boss HP for GUI

        // Example: switch between actions based on timer

        _boss.CurrentHP = currentHealth;

        // Example: reduce HP over time to see phases change automatically
        if (_boss.CurrentHP > 0 && _boss.CurrentState.StateType == BossSystem.BossStateType.Fighting)
        {

        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= (int)damage;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 500, 50), "Boss HP: " + currentHealth + "/" + maxHealth);
    }
}