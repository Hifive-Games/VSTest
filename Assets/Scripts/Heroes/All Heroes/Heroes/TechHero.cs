using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TechHero : TheHero
{
    public GameObject dronePrefab;
    public int numberOfDrones = 2;
    public float radius = 2f;
    public float rotationSpeed = 1f;
    public float floatSpeed = 1f;
    public float floatHeight = 0.5f;
    public float smoothRate = 5f;

    private float baseAngle = 0f;
    private List<GameObject> drones = new List<GameObject>();
    private List<Vector3> initialOffsets = new List<Vector3>();

    private void Start()
    {
        for (int i = 0; i < numberOfDrones; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfDrones;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            GameObject drone = Instantiate(dronePrefab, transform.position + offset, Quaternion.identity, transform);
            drones.Add(drone);
            initialOffsets.Add(offset);
        }
    }

    public void AddDrone()
    {
        numberOfDrones++;
        float angle = (numberOfDrones - 1) * Mathf.PI * 2f / numberOfDrones;
        Vector3 offset = new Vector3(Mathf.Cos(angle), 1f, Mathf.Sin(angle)) * radius;
        GameObject newDrone = Instantiate(dronePrefab, transform.position + offset, Quaternion.identity, transform);

        drones.Add(newDrone);
        initialOffsets.Add(offset);

        for (int i = 0; i < drones.Count; i++)
        {
            float newAngle = i * Mathf.PI * 2f / numberOfDrones;
            initialOffsets[i] = new Vector3(Mathf.Cos(newAngle), 1f, Mathf.Sin(newAngle)) * radius;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddDrone();
        }

        if (drones.Count > 0)
        {
            MoveDrones();
        }
    }

    private void MoveDrones()
    {
        baseAngle += rotationSpeed * Time.deltaTime;
        Vector3 currentPos = transform.position;
        int droneCount = drones.Count;

        for (int i = 0; i < droneCount; i++)
        {
            float angle = baseAngle + i * (Mathf.PI * 2f / droneCount);
            Vector3 desiredPos = currentPos + new Vector3(Mathf.Cos(angle), 1f, Mathf.Sin(angle)) * radius;
            float floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            desiredPos.y += floatOffset;

            drones[i].transform.position = Vector3.Lerp(drones[i].transform.position, desiredPos, Time.deltaTime * smoothRate);
        }
    }

   public override void SetAttackSpeed(float newRate)
{
    foreach (var drone in drones)
    {
        var weapon = drone.GetComponent<DroneWeapon>();
        if (weapon != null)
        {
            weapon.attackSpeed = Mathf.Max(0.01f, newRate);
        }
    }
}

public override void AddAttackSpeed(float newRate)
{
    foreach (var drone in drones)
    {
        var weapon = drone.GetComponent<DroneWeapon>();
        if (weapon != null)
        {
            weapon.attackSpeed *= (1f + newRate / 100f);
            weapon.attackSpeed = Mathf.Max(0.01f, weapon.attackSpeed);
        }
    }
}

public override void ReduceAttackSpeed(float newRate)
{
    foreach (var drone in drones)
    {
        var weapon = drone.GetComponent<DroneWeapon>();
        if (weapon != null)
        {
            weapon.attackSpeed *= (1f - newRate / 100f);
            weapon.attackSpeed = Mathf.Max(0.01f, weapon.attackSpeed);
        }
    }
}

public override void SetAttackRange(float newRate)
{
    foreach (var drone in drones)
    {
        var weapon = drone.GetComponent<DroneWeapon>();
        if (weapon != null)
        {
            weapon.detectionRange = Mathf.Max(0f, newRate);
        }
    }
}

public override void AddAttackRange(float newRate)
{
    foreach (var drone in drones)
    {
        var weapon = drone.GetComponent<DroneWeapon>();
        if (weapon != null)
        {
            weapon.detectionRange *= (1f + newRate / 100f);
        }
    }
}

public override void SetAttackSize(float newRate)
{
    foreach (var drone in drones)
    {
        var weapon = drone.GetComponent<DroneWeapon>();
        if (weapon != null)
        {
            weapon.attackSize = Mathf.Max(0.1f, newRate);
        }
    }
}

public override void AddAttackSize(float newRate)
{
    foreach (var drone in drones)
    {
        var weapon = drone.GetComponent<DroneWeapon>();
        if (weapon != null)
        {
            weapon.attackSize *= (1f + newRate / 100f);
        }
    }
}

public override void SetAttackAmount(float newRate)
{
    foreach (var drone in drones)
    {
        var weapon = drone.GetComponent<DroneWeapon>();
        if (weapon != null)
        {
            weapon.attackAmount = Mathf.Max(1, Mathf.RoundToInt(newRate));
        }
    }
}

public override void AddAttackAmount(float newRate)
{
    foreach (var drone in drones)
    {
        var weapon = drone.GetComponent<DroneWeapon>();
        if (weapon != null)
        {
            weapon.attackAmount += Mathf.RoundToInt(newRate);
            weapon.attackAmount = Mathf.Max(1, weapon.attackAmount);
        }
    }
}
}
