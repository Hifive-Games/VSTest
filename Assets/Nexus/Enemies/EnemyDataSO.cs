using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]

public class EnemyDataSO: ScriptableObject
{
    public int maxHealth;
    public int damage;
    public float speed;

    public int score;

    public GameObject deathEffect;

    public int experience;
}
