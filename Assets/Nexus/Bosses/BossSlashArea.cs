using System.Collections;
using UnityEngine;

public class BossSlashArea : MonoBehaviour
{
    [SerializeField] private float activeDuration = 1f;
    [SerializeField] private int damage = 10;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        if (spriteRenderer)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
        }
        // Start coroutine to fade in
        StartCoroutine(DisableSlashArea());
    }

    void OnDisable()
    {
        if (spriteRenderer)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
        }
    }

    public bool PlayerInSlashArea(GameObject player)
    {
        // Check if player is within the bounds of the slash area
        Collider playerCollider = player.GetComponent<Collider>();
        if (playerCollider)
        {
            return playerCollider.bounds.Intersects(GetComponent<Collider>().bounds);
        }
        return false;
    }

    private IEnumerator DisableSlashArea()
    {
        float timer = activeDuration;
        while (timer > 0)
        {
            if (spriteRenderer)
            {
                spriteRenderer.color = new Color(1, 1, 1, 1 - (timer / activeDuration));
            }
            timer -= Time.deltaTime;
            yield return null;
        }

        if (PlayerInSlashArea(FindFirstObjectByType<CharacterController>().gameObject))
        {
            TheHeroDamageManager playerDamageManager = FindFirstObjectByType<TheHeroDamageManager>();
            if (playerDamageManager != null)
            {
                //playerDamageManager.TakeDamage(damage); // Apply damage to the player
            }
        }
        gameObject.SetActive(false);
    }
}