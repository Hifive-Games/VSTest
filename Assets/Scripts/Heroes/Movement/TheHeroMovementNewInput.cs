using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TheHeroMovementNewInput : MonoBehaviour
{
    private PlayerInputs playerInputs;
    private Vector2 inputVector2;
    private Vector3 movementDirection;

    private CharacterController characterController;

    [SerializeField] private float moveSpeed = 5f;
    private Animator animator; // Animator bileşeni

    [SerializeField] private float rotationSpeed = 5f; // Yüz dönüş hızı

    private void Awake()
    {
        InitializeInputSystem();
        InitializeComponents();
    }

    #region Initialize
    private void InitializeComponents()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void InitializeInputSystem()
    {
        playerInputs = new PlayerInputs();
        playerInputs.PlayerMovement.Enable();
    }
    #endregion

    public void AddMoveSpeed(float value)
    {
        moveSpeed =value+moveSpeed;
    }
    public void SetMoveSpeed(float value)
    {
        moveSpeed =value;
    }
    
    private Vector2 GetMovementVectorNormalized()
    {
        return playerInputs.PlayerMovement.Move.ReadValue<Vector2>().normalized;
    }

    private void Update()
    {
        MovePlayer();
        FaceNearestEnemy(); // En yakın düşmana yüzünü döndür
    }

    private void MovePlayer()
    {
        inputVector2 = GetMovementVectorNormalized();
        movementDirection = new Vector3(inputVector2.x, 0f, inputVector2.y); // Y eksenini sıfırda tutarak düzlemde hareket sağlıyoruz.

        // CharacterController ile hareket
        characterController.Move(movementDirection * moveSpeed * Time.deltaTime);

        animator.SetFloat("VelocityX", movementDirection.x);
        animator.SetFloat("VelocityZ", movementDirection.z);
    }

    private void FaceNearestEnemy()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            Vector3 directionToEnemy = nearestEnemy.transform.position - transform.position;
            directionToEnemy.y = 0f; // Y eksenini sıfırda tutarak sadece yatay düzlemde dönüş yapıyoruz

            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
    
    //Reducelar
    public void ReduceMoveSpeed(float value)
    {
        moveSpeed =value-moveSpeed;
    }
}
