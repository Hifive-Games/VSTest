using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]

public class WeaponDataSO : ScriptableObject
{
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    public int damage;
    public float bulletSpeed;
    public float bulletLifeTime;
}
