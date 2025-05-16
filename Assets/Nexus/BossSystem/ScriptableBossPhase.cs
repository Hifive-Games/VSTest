using UnityEngine;

public abstract class ScriptableBossPhase : ScriptableObject, IState<BossController>
{
    public abstract void Enter(BossController boss);
    public abstract void Tick(BossController boss);
    public abstract void Exit(BossController boss);
    public abstract ScriptableBossPhase GetPhase();

    public virtual void BossPhaseUI(BossController boss)
    {
        //grt the phase number
        int phaseNumber = boss.Phases.IndexOf(this) + 1;
        BossHealthBarUI.Instance.SetBossPhase(phaseNumber);
    }
}