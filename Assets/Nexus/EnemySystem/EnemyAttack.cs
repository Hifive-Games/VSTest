using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    //this will spawn when enemy try to attack, and it will take damage, range and position from enemy and will damage the player if its in attack radius. Will use Object Pooling for this prefab so do everything in the enable and disable. We can use courotine for the damage check. if player still in range before this attack object is destroyed, it will damage the player.
    private float damage;
    private float attackRange;
    private float attackSpeed;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Color startColor;
    private Color endColor;

    public void Start()
    {
        //this will get the sprite renderer component
        startColor = new Color(1f, 0f, 0f, 0f);
        endColor = new Color(1f, 0f, 0f, 1f);
    }

    public void SetAttackData(int dmg, float range, float speed)
    {
        damage = dmg;
        attackRange = range;
        attackSpeed = speed;
        // no repositioning here – spawned at enemy already


        //this will set the color of the sprite to red
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        //this will start the coroutine to check if player is in range
        StartCoroutine(CheckPlayerInRange(attackSpeed));
    }

    private IEnumerator CheckPlayerInRange(float attackSpeed)
    {

        //Increase sprite alpha with time (also attack speed)
        float elapsedTime = 0f;
        float duration = attackSpeed;

        //adjust scale of the attack prefab
        transform.localScale = new Vector3(attackRange, attackRange / 2f, attackRange);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }
        //check if player is in range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange/2);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<TheHeroDamageManager>(out TheHeroDamageManager dm))
            {
                //this will damage the player if its in range
                dm.TakeDamage((int)damage);
                //Debug.Log("Player is in range");
            }
        }

        //then return to object pool
        yield return new WaitForSeconds(0.1f);
        ObjectPooler.Instance.ReturnObject(gameObject);
        StopAllCoroutines();
    }
}