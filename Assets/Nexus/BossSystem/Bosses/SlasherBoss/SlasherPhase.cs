using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Boss/Phase/Slasher", fileName = "NewSlasherPhase")]
public class SlasherPhase : ScriptableBossPhase
{
    [Header("Melee Chase")]
    public float meleeRange = 2f;
    public float meleeCooldown = 1f;
    public float roamRadius = 5f;

    [Header("Attacks/Spells (optional)")]
    public bool enableAttacks = false;
    public List<AttackInfo> attackInfos;

    [Header("Buffs")]
    public float speedMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float meleeCooldownMultiplier = 1f;

    [Header("Next Phase")]
    [Tooltip("The health percentage at which to transition to the next phase. 100% = full health, 0% = dead")]
    [Range(100f, 0f)]
    public int healthThreshold = 100;
    public ScriptableBossPhase nextPhase;

    private float _currentHealth;
    private float _maxHealth;

    // internal state
    float _meleeTimer;
    bool _roaming;
    Vector3 _roamTarget;
    float _origSpeed, _origCooldown;
    int _origDamage;

    private GameObject _player;

    public override void Enter(BossController boss)
    {
        BossPhaseUI(boss);

        _player = FindAnyObjectByType<CharacterController>().gameObject;

        // store & apply buffs
        _origSpeed = boss.speed;
        _origDamage = boss.damage;
        _origCooldown = meleeCooldown;
        boss.speed *= speedMultiplier;
        boss.damage = Mathf.RoundToInt(boss.damage * damageMultiplier);
        meleeCooldown *= meleeCooldownMultiplier;

        // store health
        _currentHealth = boss.currentHealth;
        _maxHealth = boss.maxHealth;



        //fill the attack list if Attacks are enabled
        if (enableAttacks)
        {
            // clear existing attacks
            for (int i = 0; i < boss.Attacker.AttackCount; i++)
            {
                boss.Attacker.RemoveAttack(i);
            }
            for (int i = 0; i < attackInfos.Count; i++)
            {
                boss.Attacker.AddAttack(i,
                                        attackInfos[i].attackWarningPrefab,
                                        attackInfos[i].attackDelay,
                                        attackInfos[i].attackPrefab,
                                        attackInfos[i].attackDuration,
                                        attackInfos[i].attackCount,
                                        attackInfos[i].attackDamage,
                                        attackInfos[i].attackRange,
                                        attackInfos[i].attackArea);
            }
        }

        _meleeTimer = 0f;
        _roaming = false;
    }

    public override void Tick(BossController boss)
    {
        // 1) always tick down your melee cooldown
        _meleeTimer -= Time.deltaTime;

        Vector3 bossPos = new Vector3(boss.transform.position.x, 2, boss.transform.position.z);
        Vector3 playerPos = new Vector3(_player.transform.position.x, 1, _player.transform.position.z);
        float dist = Vector3.Distance(bossPos, playerPos);

        if (_roaming)
        {
            boss.Mover.MoveTo(_roamTarget, boss.speed);
            // once we reach the roam point…
            if (Vector3.Distance(bossPos, _roamTarget) < .1f)
            {
                if (_meleeTimer <= 0f)
                {
                    // CD is ready: stop roaming and go back to chase/attack logic
                    _roaming = false;
                }
                else
                {
                    // CD still cooling: pick another roam target
                    float bossY = boss.transform.position.y;
                    Vector3 playerXZ = new Vector3(_player.transform.position.x, 1f, _player.transform.position.z);
                    Vector3 roamPoint;
                    int tries = 0;
                    do
                    {
                        Vector2 rnd = Random.insideUnitCircle * roamRadius;
                        roamPoint = new Vector3(playerXZ.x + rnd.x,
                                                 bossY,
                                                 playerXZ.z + rnd.y);
                        tries++;
                    }
                    while (tries < 10 &&
                           Vector3.Distance(new Vector3(roamPoint.x, 0f, roamPoint.z), playerXZ) <= meleeRange);

                    _roamTarget = roamPoint;
                }
            }
        }
        else
        {
            // chase only if outside meleeRange
            if (dist > meleeRange)
                boss.Mover.MoveTo(playerPos, boss.speed);

            // melee attack only when in range AND CD is ready
            if (dist <= meleeRange && _meleeTimer <= 0f)
            {
                int attackId = Random.Range(0, attackInfos.Count);
                boss.Attacker.DoAttack(attackId, _player.transform);
                _meleeTimer = meleeCooldown;

                // pick initial roam target after attack
                float bossY = boss.transform.position.y;
                Vector3 playerXZ = new Vector3(_player.transform.position.x, 1f, _player.transform.position.z);
                Vector3 roamPoint;
                int tries = 0;
                do
                {
                    Vector2 rnd = Random.insideUnitCircle * roamRadius;
                    roamPoint = new Vector3(playerXZ.x + rnd.x,
                                             bossY,
                                             playerXZ.z + rnd.y);
                    tries++;
                }
                while (tries < 10 &&
                       Vector3.Distance(new Vector3(roamPoint.x, 0f, roamPoint.z), playerXZ) <= meleeRange);

                _roamTarget = roamPoint;
                _roaming = true;
            }
        }

        _currentHealth = boss.currentHealth;

        float hpPct = _currentHealth / _maxHealth * 100f;

        // check for phase change
        if (hpPct <= healthThreshold)
        {
            // change phase
            boss.PhaseMachine.ChangeState(nextPhase);
        }

        // death
        if (_currentHealth <= 0)
        {
            boss.StateMachine.ChangeState(boss.DyingState);
            Die();
        }
    }

    public override void Exit(BossController boss)
    {
        // restore originals
        boss.speed = _origSpeed;
        boss.damage = _origDamage;
        meleeCooldown = _origCooldown;

        if(boss.currentHealth <= 0)
        {
            Die();
        }
    }



    public void Die()
    {
        Debug.Log($"<color=red>Slasher is dead</color>");
        GameEvents.OnZeroHealth?.Invoke();
    }

    public override ScriptableBossPhase GetPhase()
    {
        return this;
    }
}