using UnityEngine;

public class CharaterMovement : MonoBehaviour
{
    public static CharaterMovement Instance;
    public float currentSpeed;

    [SerializeField]

    GameObject model;

    CharacterController controller;

    public bool autoTargeting = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        LookAtMouse(model);
    }

    public void Move()
    {
        // Get input from player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move player
        Vector3 move = transform.right * x + transform.forward * z;

        // Normalize the move vector if it's not zero to prevent faster diagonal movement
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        // Apply movement
        controller.Move(move * currentSpeed * Time.deltaTime);
    }


    public void LookAtMouse(GameObject model)
    {
        if(autoTargeting) return;
        // Get mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float distance;

        // Check if ray hits the ground, but the model must rotate around players models edge
        if (ground.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            Vector3 direction = point - model.transform.position;
            direction.y = 0;

            // Rotate model
            model.transform.forward = direction;
        }
    }

    public void SetSpeed(float speed)
    {
        currentSpeed += speed;
        PlayerMagnet.Instance.minPullSpeed = currentSpeed;
    }
}