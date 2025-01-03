using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this will be abstract class for all enemies

public abstract class Enemies : MonoBehaviour
{
    public EnemyDataSO enemySO;
    public int maxHealth;
    public int damage;
    public float speed;
    public int currentHealth;
    public int score;
    public int experience;

    public bool hitBySpell = false;

    public GameObject expPrefab;

    protected GameObject player;

    private void Initialize()
    {
        maxHealth = enemySO.maxHealth;
        damage = enemySO.damage;
        speed = enemySO.speed;
        score = enemySO.score;
        experience = enemySO.experience;

        if (player == null)
        {
            player = Player.Instance.gameObject;
        }
    }

    protected void OnEnable()
    {
        currentHealth = maxHealth;
        Initialize();
    }

    protected void OnDisable()
    {
        player = null;
    }

    private void ResetHitBySpell()
    {
        print("ResetHitBySpell");
        hitBySpell = false;
    }

    public void TakeDamage(int damage)
    {
        if (hitBySpell)
        {
            Invoke("ResetHitBySpell", 2f);
        }
        else
            currentHealth -= damage;
        
            
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        ObjectPooler.Instance.ReturnObject(gameObject);
        GameManager.Instance.score += enemySO.score;

        GameObject exp = ObjectPooler.Instance.GetObject(expPrefab);
        exp.transform.position = transform.position;
        exp.GetComponent<Experiance>().experience = enemySO.experience;

        InterfaceManager.Instance.AddkillCount();

        InterfaceManager.Instance.CreateKillCountTextShake();

        PlayerMagnet.Instance.audioSource.PlayOneShot(PlayerMagnet.Instance.killSfx);
    }
}
