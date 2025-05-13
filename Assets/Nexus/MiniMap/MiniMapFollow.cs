using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        target = FindAnyObjectByType<CharacterController>().transform;
    }

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 newPos = target.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }
}
