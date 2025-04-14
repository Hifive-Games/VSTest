using UnityEngine;

public class BossActionSystem : MonoBehaviour
{
    private enum BossActionState
    {
        MoveToPlayer,
        Attack,
        WaitAfterAttack,
        MoveToRandom,
        WaitAfterRandom
    }

    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTimeAfterAttack = 2f;
    [SerializeField] private float waitTimeAfterMoveRandom = 2f;
    [SerializeField] private Transform randomPoint = null;
    [SerializeField] private GameObject slashAreaPrefab;  // Added

    private BossActionState currentState;
    private float timer;
    private GameObject player;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        player = GameObject.FindAnyObjectByType<CharacterController>().gameObject;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            return;
        }
        currentState = BossActionState.MoveToPlayer;
        randomPoint = new GameObject().transform;
        timer = 0f;
    }

    void Update()
    {
        switch (currentState)
        {
            case BossActionState.MoveToPlayer:
                MoveTo(player.transform.position);
                if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
                {
                    currentState = BossActionState.Attack;
                }
                break;

            case BossActionState.Attack:
                SlashAttack();
                timer = waitTimeAfterAttack;
                currentState = BossActionState.WaitAfterAttack;
                break;

            case BossActionState.WaitAfterAttack:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    currentState = BossActionState.MoveToRandom;
                }
                break;

            case BossActionState.MoveToRandom:
                MoveTo(randomPoint.position);
                if (Vector3.Distance(transform.position, randomPoint.position) < .5f)
                {
                    timer = waitTimeAfterMoveRandom;
                    currentState = BossActionState.WaitAfterRandom;
                }
                break;

            case BossActionState.WaitAfterRandom:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    currentState = BossActionState.MoveToPlayer;
                }
                break;
        }

        transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
    }

    private void MoveTo(Vector3 targetPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void SlashAttack()
    {
        if (slashAreaPrefab != null)
        {
            slashAreaPrefab.SetActive(true);
            RandomizePoint();
        }
    }

    public Transform RandomizePoint()
    {
        // Randomize the point within the range of the player
        randomPoint.position = player.transform.position + new Vector3(Random.Range(-5f, 5f), 2f, Random.Range(-5f, 5f));
        randomPoint.position = new Vector3(randomPoint.position.x, 2f, randomPoint.position.z);
        return randomPoint;
    }
}