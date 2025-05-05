using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

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

    public override void OnEnable()
    {
        Mover = GetComponent<BossMovement>();
        Attacker = GetComponent<BossAttack>();
        Caster = GetComponent<BossCaster>();
        _anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Initialize(enemySO);
        _stateMachine.Initialize(this, spawningState);
    }

    public override void Update()
    {
        _stateMachine.Update();
    }

    [Button("Deal Damage")]
    public void DealDamage(float amount = 10f)
    {
        if (amount <= 0) return;

        currentHealth -= Mathf.RoundToInt(amount);
        Debug.Log($"Boss took {amount} damage. Current health: {currentHealth}");
    }
}