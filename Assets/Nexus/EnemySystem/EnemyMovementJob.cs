using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

public struct EnemyMovementJob : IJob
{
    public Vector3 enemyPos;
    public Vector3 playerPos;
    public float speed;
    public float deltaTime;
    [ReadOnly] public NativeArray<Vector3> obstaclePositions;
    public float avoidanceWeight; // how strongly the avoidance influences movement

    // Outputs
    public Vector3 targetPos;
    public Vector3 moveDirection;

    public void Execute()
    {
        // Compute obstacle avoidance vector using positions passed from the main thread.
        Vector3 avoidance = Vector3.zero;
        for (int i = 0; i < obstaclePositions.Length; i++)
        {
            // Compute a vector away from the obstacle.
            Vector3 diff = enemyPos - obstaclePositions[i];
            float distSq = diff.sqrMagnitude;
            if (distSq > 0.001f)
            {
                // Weight by inverse distance.
                avoidance += diff.normalized / Mathf.Sqrt(distSq);
            }
        }
        if (avoidance != Vector3.zero)
        {
            avoidance = avoidance.normalized * avoidanceWeight;
        }

        // Compute the primary movement direction toward the player.
        Vector3 primaryDirection = (playerPos - enemyPos).normalized;
        // Blend the primary direction with the avoidance vector.
        Vector3 combinedDirection = (primaryDirection + avoidance).normalized;
        moveDirection = combinedDirection;

        // Compute the target position.
        targetPos = enemyPos + combinedDirection * speed * deltaTime;
    }
}