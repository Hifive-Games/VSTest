using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Phase/Generic", fileName = "NewGenericPhase")]
public class GenericPhase : ScriptableBossPhase
{
    [Header("Timing")]
    [Tooltip("Duration in seconds (<=0 to ignore)")]
    public float duration = 5f;
    float _timer;

    [Header("Health Threshold")]
    [Range(0f, 1f)]
    [Tooltip("HP% at or below which to transition (<=0 to ignore)")]
    public float healthThreshold = 0f;

    [Header("Movement")]
    [Tooltip("Offset from boss start position; leave zero if you don’t want movement")]
    public Vector3 moveOffset = Vector3.zero;
    public float moveSpeed = 3f;
    Vector3 _moveTargetWorld;

    [Header("Attack")]
    public int attackId = 0;
    public float attackInterval = 1f;
    float _attackTimer;

    [Header("Next Phase")]
    [Tooltip("SO to switch into when timer expires or HP threshold hit")]
    public ScriptableBossPhase nextPhase;

    bool _transitioned;

    public override void Enter(BossController boss)
    {
        Debug.Log($"Entering phase: {name}");
        _timer = duration;
        _attackTimer = attackInterval;
        _transitioned = false;

        // compute a real-world target once
        _moveTargetWorld = boss.transform.position + moveOffset;
    }

    public override void Tick(BossController boss)
    {
        // countdown
        if (duration > 0f) _timer -= Time.deltaTime;
        if (attackInterval > 0f) _attackTimer -= Time.deltaTime;

        // movement
        if (moveOffset != Vector3.zero && boss.Mover != null)
            boss.Mover.MoveTo(_moveTargetWorld, moveSpeed);

        // attack
        if (boss.Attacker != null && _attackTimer <= 0f)
        {
            boss.Attacker.DoAttack(attackId, boss.transform);
            _attackTimer = attackInterval;
        }

        // transition conditions (only once)
        if (!_transitioned && nextPhase != null)
        {
            bool timeUp = duration > 0f && _timer <= 0f;
            bool lowHp = healthThreshold > 0f
                          && boss.maxHealth > 0f
                          && (boss.currentHealth / boss.maxHealth) <= healthThreshold;

            if (timeUp || lowHp)
            {
                _transitioned = true;
                boss.PhaseMachine.ChangeState(nextPhase);
                return;
            }
        }
    }

    public override void Exit(BossController boss) { }
}