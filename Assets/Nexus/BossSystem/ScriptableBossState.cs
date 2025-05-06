using UnityEngine;

public abstract class ScriptableBossState : ScriptableObject, IState<BossController>
{
    public abstract void Enter(BossController boss);
    public abstract void Tick(BossController boss);
    public abstract void Exit(BossController boss);
}