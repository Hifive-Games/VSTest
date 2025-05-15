using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BossMovement))]
[RequireComponent(typeof(BossAttack))]
[RequireComponent(typeof(BossCaster))]
public class BossController : Enemy
{
    // --- Core FSM ---
    StateMachine<BossController> _stateMachine = new StateMachine<BossController>();
    StateMachine<BossController> _phaseMachine = new StateMachine<BossController>();
    public StateMachine<BossController> StateMachine => _stateMachine;
    public StateMachine<BossController> PhaseMachine => _phaseMachine;

    // --- Movement / Attack services ---
    public IBossMover Mover { get; private set; }
    public IBossAttacker Attacker { get; private set; }
    public IBossSpellCaster Caster { get; private set; }

    // --- Assign these in the Inspector ---
    [Header("Boss States (ScriptableObjects)")]
    [SerializeField] ScriptableBossState spawningState;
    [SerializeField] ScriptableBossState fightingState;
    [SerializeField] ScriptableBossState dyingState;

    public ScriptableBossState SpawningState => spawningState;
    public ScriptableBossState FightingState => fightingState;
    public ScriptableBossState DyingState => dyingState;

    [Header("Phases (in order)")]
    [SerializeField] List<ScriptableBossPhase> phases;
    public List<ScriptableBossPhase> Phases => phases;

    Animator _anim;

    public Animator AnimatorComponent => _anim;

    public void Start()
    {
        SetBossUI();
    }

    public override void OnEnable()
    {
        Mover = GetComponent<BossMovement>();
        Attacker = GetComponent<BossAttack>();
        Caster = GetComponent<BossCaster>();
        _anim = GetComponent<Animator>();
        Initialize(enemySO);
        _stateMachine.Initialize(this, spawningState);
        base.OnEnable();
    }

    public override void OnDisable()
    {
        BossHealthBarUI.Instance.ActivateHealthBarUI(false);
    }

    public void SetBossUI()
    {
        BossHealthBarUI.Instance.ActivateHealthBarUI(true);
        //boss name but trim Boss in the name
        BossHealthBarUI.Instance.SetBossName(enemySO.name.Replace("Boss", ""));
    }

    public override void Update()
    {
        _stateMachine.Update();
    }

    [Button("Deal Damage")]
    public void DealDamage(float amount = 40f)
    {
        //check if the boss is in the fighting state
        if (ReferenceEquals(_stateMachine.CurrentState, fightingState))
        {
            TakeDamage((int)amount);
        }
        else
        {
            Debug.LogWarning("Boss is not in the fighting state.");
        }
    }

    public override void TakeDamage(int damage)
    {
        if (ReferenceEquals(_stateMachine.CurrentState, fightingState))
        {
            base.TakeDamage(damage);
        }

        //Adjust the health bar
        float healthPercentage = (float)currentHealth / maxHealth;
        BossHealthBarUI.Instance.SetHealthBar(healthPercentage);

        if(currentHealth <= 0)
        {
            //change to dying state
            _stateMachine.ChangeState(dyingState);
        }
    }

    public override void Die()
    {
        SFXManager.Instance.PlayAt(SFX.EnemyDie);

        EnemySpawner.Instance.RemoveEnemy(gameObject);

        Vector3 position = new Vector3(transform.position.x, .5f, transform.position.z);
        GlobalGameEventManager.Instance.Notify("EnemyDied", experience, position, expPrefab);

        Debuffs.Clear();
        EnemySpawner.Instance.AddKill();
    }

}