using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Boss/State/Dying", fileName = "NewDyingState")]
public class DyingState : ScriptableBossState
{
    public float deathDelay = 1.5f;
    [Tooltip("Set this in Inspector once you add a Death animation")]
    public string deathAnimTrigger = "";

    public GameObject bossDropPrefab;

    public override void Enter(BossController boss)
    {
        boss.StartCoroutine(DeathRoutine(boss));
    }

    IEnumerator DeathRoutine(BossController boss)
    {
        if (!string.IsNullOrEmpty(deathAnimTrigger))
            boss.AnimatorComponent.SetTrigger(deathAnimTrigger);
        yield return new WaitForSeconds(deathDelay);
        //Destroy(boss.gameObject);
    }

    public override void Tick(BossController boss) { }
    public override void Exit(BossController boss) { }
}