using UnityEngine;

[CreateAssetMenu(menuName = "Boss/State/Fighting", fileName = "NewFightingState")]
public class FightingState : ScriptableBossState
{
    public override void Enter(BossController boss)
    {
        if (boss.Phases.Count > 0)
            boss.PhaseMachine.Initialize(boss, boss.Phases[0]);
    }

    public override void Tick(BossController boss)
    {
        // nested phase logic
        boss.PhaseMachine.Update();

        // exit fight when dead
        if (boss.currentHealth <= 0f)
            boss.StateMachine.ChangeState(boss.DyingState);
    }

    public override void Exit(BossController boss) { }
}