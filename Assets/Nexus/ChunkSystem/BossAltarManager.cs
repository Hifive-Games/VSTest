using UnityEngine;

public class BossAltarManager : MonoBehaviour
{
    //this script is used to after the boss altar is interacted
    //it will create boss arena and spawn the boss
    //it will also handle the boss arena and boss despawn

    public GameObject bossArenaPrefab;
    public GameObject bossPrefab;

    private GameObject bossArena;
    private GameObject boss;

    public void ActivateBossAltar(Vector3 position)
    {
        bossArena = Instantiate(bossArenaPrefab, position, Quaternion.identity);
        boss = Instantiate(bossPrefab, position, Quaternion.identity);
    }

    public void DeactivateBossAltar()
    {
        Destroy(bossArena);
        Destroy(boss);
    }

    public void BossDefeated()
    {
        DeactivateBossAltar();
    }
}