using UnityEngine;

public class BossMovement : MonoBehaviour, IBossMover
{
    public void MoveTo(Vector3 worldPos, float speed)
    {
        Vector3 current = transform.position;
        Vector3 target = new Vector3(worldPos.x, current.y, worldPos.z);
        Vector3 delta = target - current;

        // bail out if we’re basically there
        if (delta.sqrMagnitude < 0.01f)
            return;

        // compute the step (won’t overshoot)
        Vector3 step = Vector3.MoveTowards(
            current,
            target,
            speed * Time.deltaTime
        ) - current;

        // move
        transform.position += step;
        transform.forward = step.normalized;
    }
}