using System.Collections;
using System.Net;
using Unity.Mathematics;
using UnityEngine;
/// <summary>
/// Will change when DOTWEEN is implemented
/// </summary>
public class PlayerMagnet : MonoBehaviour
{
    public float magnetRange;
    public float superMagnetRange = 10000f;
    public float normalMagnetRange = 8f;
    public float collectableTime = 30f;
    public static PlayerMagnet Instance;

    public bool test = false;

    public float minPullSpeed = 10f;

    public AudioSource audioSource;
    public AudioClip expSfx;
    public AudioClip killSfx;

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

        magnetRange = normalMagnetRange;
    }

    public void SetMagnetRange(float range)
    {
        normalMagnetRange += range;
        magnetRange = normalMagnetRange;
    }

    private void Update()
    {
        if (test)
        {
            test = false;
            SetSuperMagnet();
        }
        CheckForExperiance();
    }

    public void SetSuperMagnet()
    {
        magnetRange = superMagnetRange;
        Invoke("SetNormalMagnet", collectableTime);
    }

    private void SetNormalMagnet()
    {
        magnetRange = normalMagnetRange;
    }

    public void CheckForExperiance()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, magnetRange);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out ExperienceParticle experiance))
            {
                MoveExperiance(experiance);
            }
        }
    }

    private void MoveExperiance(ExperienceParticle experienceParticle)
    {
        experienceParticle.transform.position = Vector3.MoveTowards(experienceParticle.transform.position, transform.position, math.max(minPullSpeed, Vector3.Distance(experienceParticle.transform.position, transform.position)) * Time.deltaTime);
    }

    private void  OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}
