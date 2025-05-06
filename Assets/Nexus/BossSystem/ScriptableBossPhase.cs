using UnityEngine;

public abstract class ScriptableBossPhase : ScriptableObject, IState<BossController>
{
    public abstract void Enter(BossController boss);
    public abstract void Tick(BossController boss);
    public abstract void Exit(BossController boss);
    public abstract ScriptableBossPhase GetPhase();

    public virtual void BossPhaseUI(BossController boss)
    {
        //set the boss phase number but reverse the index
        int totalPhases = boss.Phases.Count;
        int phaseNumber = totalPhases - boss.Phases.IndexOf(this);
        
        BossHealthBarUI.Instance.SetBossPhase(phaseNumber);
    }
}