using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        target = FindObjectOfType<CharacterController>().transform;
    }

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 newPos = target.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }
}
