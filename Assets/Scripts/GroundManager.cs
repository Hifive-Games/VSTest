using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public GameObject groundTilePrefab;
    public int gridSize = 5; // Number of tiles in each direction
    public float groundTileSize = 10f; // Size of each tile
    private Transform playerTransform;
    private Dictionary<Vector2, GameObject> groundTiles;
    private Vector2 playerGridPosition;

    void Start()
    {
        playerTransform = Player.Instance.transform;
        groundTiles = new Dictionary<Vector2, GameObject>();
        playerGridPosition = GetGridPosition(playerTransform.position);

        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int z = -gridSize; z <= gridSize; z++)
            {
                Vector2 gridPos = new Vector2(x, z);
                Vector3 worldPos = GridToWorldPosition(gridPos);
                GameObject tile = Instantiate(groundTilePrefab, worldPos, Quaternion.identity);
                groundTiles[gridPos] = tile;
            }
        }
    }

    void Update()
    {
        Vector2 newPlayerGridPosition = GetGridPosition(playerTransform.position);

        if (newPlayerGridPosition != playerGridPosition)
        {
            playerGridPosition = newPlayerGridPosition;
            RepositionGroundTiles();
        }
    }

    Vector2 GetGridPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / groundTileSize);
        int z = Mathf.FloorToInt(position.z / groundTileSize);
        return new Vector2(x, z);
    }

    Vector3 GridToWorldPosition(Vector2 gridPos)
    {
        float x = gridPos.x * groundTileSize;
        float z = gridPos.y * groundTileSize;
        return new Vector3(x, 0, z);
    }

    void RepositionGroundTiles()
    {
        List<Vector2> keys = new List<Vector2>(groundTiles.Keys);

        foreach (Vector2 gridPos in keys)
        {
            Vector2 offset = gridPos - playerGridPosition;

            if (Mathf.Abs(offset.x) > gridSize || Mathf.Abs(offset.y) > gridSize)
            {
                Vector2 newGridPos = playerGridPosition + new Vector2(
                    Mathf.Sign(offset.x) * gridSize * -2,
                    Mathf.Sign(offset.y) * gridSize * -2
                );

                GameObject tile = groundTiles[gridPos];
                tile.transform.position = GridToWorldPosition(newGridPos);
                groundTiles.Remove(gridPos);
                groundTiles[newGridPos] = tile;
            }
        }
    }
}
