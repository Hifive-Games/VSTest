using UnityEngine;
public enum EnemyType
{
    BasicMelee,
    Ranged,
    FastFragile,
    SlowHardHitter
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyDataSO : ScriptableObject
{
    [Header("Enemy Settings")]
    public EnemyType enemyType;
    public int ID;
    public string enemyName;
    public Sprite enemySprite;
    public GameObject enemyPrefab;

    [Header("Stats")]
    public int maxHealth;
    public int damage;
    public float speed;
    public int score;
    public float attackRange;
    public float attackSpeed;

    [Header("Experience")]
    public int experience;
    public GameObject deathEffect;
}
