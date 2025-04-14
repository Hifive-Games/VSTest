using System.Collections;
using UnityEngine;

public class BossHitArea : MonoBehaviour
{
    private Color color;
    private SpriteRenderer spriteRenderer;
    private GameObject player;

    public float warningDuration = 1f;
    public float fadeDuration = 10f;
    public int damage = 10;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
        gameObject.SetActive(false);

        player = FindObjectOfType<CharacterController>().gameObject;
    }

    void OnEnable()
    {
        // Reset color and start the warning phase
        color.a = 0;
        spriteRenderer.color = color;
        gameObject.transform.position = player.transform.position;
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(WarningPhase());
    }

    private IEnumerator WarningPhase()
    {
        while (color.a < 1)
        {
            if(color.a < 0.5f)
            {
                gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                gameObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }
            // Gradually fade in the warning area
            color.a += Time.deltaTime / warningDuration;
            gameObject.transform.Rotate(0, 0, Mathf.Sin(Time.time * 10) * 0.5f);
            spriteRenderer.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        // Trigger the hit and start fading out
        Hit();
        StartCoroutine(FadeOut());
    }

    private void Hit()
    {
        // Enable the collider to detect hits
        Collider collider = GetComponent<Collider>();
        collider.enabled = true;

        // Deal damage to player if in the area
        if (player != null && collider.bounds.Contains(player.transform.position))
        {
            TheHeroDamageManager playerDamageManager = player.GetComponent<TheHeroDamageManager>();
            if (playerDamageManager != null)
            {
                //playerDamageManager.TakeDamage(damage);
            }
        }

        // Disable the collider after the hit
        collider.enabled = false;
    }

    private IEnumerator FadeOut()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        // Gradually fade out the hit area
        float fadeStep = color.a / fadeDuration;
        while (color.a > 0)
        {
            color.a -= fadeStep * Time.deltaTime;
            spriteRenderer.color = color;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            // Deal damage to player
            player.TakeDamage(damage);
        }
    }
}