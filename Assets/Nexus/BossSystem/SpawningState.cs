using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Boss/State/Spawning", fileName = "NewSpawningState")]
public class SpawningState : ScriptableBossState
{
    public float spawnDelay = 1f;
    [Tooltip("Set this in Inspector once you add a Spawn animation")]
    public string spawnAnimTrigger = "";

    public override void Enter(BossController boss)
    {
        boss.StartCoroutine(SpawnRoutine(boss));
    }

    IEnumerator SpawnRoutine(BossController boss)
    {
        if (!string.IsNullOrEmpty(spawnAnimTrigger))
            boss.AnimatorComponent.SetTrigger(spawnAnimTrigger);
        yield return new WaitForSeconds(spawnDelay);
        boss.StateMachine.ChangeState(boss.FightingState);
    }

    public override void Tick(BossController boss) { }
    public override void Exit(BossController boss) { }
}