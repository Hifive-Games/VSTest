using UnityEngine;

public interface IBossAttacker
{
    void AddAttack(int attackId,
                   GameObject warningPrefab,
                   float attackDelay,
                   GameObject attackPrefab,
                   float attackDuration,
                   int attackCount,
                   float attackDamage,
                   float attackRange,
                   float attackArea);

    void DoAttack(int attackId, Transform origin);
    
    void RemoveAttack(int attackId);
    int AttackCount { get; }
}