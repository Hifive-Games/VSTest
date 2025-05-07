using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMagnet : MonoBehaviourSingleton<PlayerMagnet>
{
    public float magnetRange;
    public float superMagnetRange = 10000f;
    public float normalMagnetRange = 2f;
    public float collectableTime = 30f;

    public bool SuperMagnet = false;

    public float minPullSpeed = 10f;

    private Collider magnetCollider;
    private Rigidbody magnetRigidbody;

    public void OnEnable()
    {
        Set();
    }
    public void Set()
    {
        gameObject.transform.SetParent(TheHero.Instance.transform);

        magnetRange = normalMagnetRange;

        magnetCollider = GetComponent<Collider>();
        magnetCollider.isTrigger = true;

        magnetRigidbody = GetComponent<Rigidbody>();
        magnetRigidbody.isKinematic = true;

        SetMagnetRange(magnetRange);
    }

    public void SetMagnetRange(float range)
    {
        gameObject.transform.localScale = new Vector3(range, range, range);
    }

    void Update()
    {
        if (SuperMagnet)
        {
            SuperMagnet = false;
            SetSuperMagnet();
        }
    }

    public void SetSuperMagnet()
    {
        SetMagnetRange(superMagnetRange);

        Invoke("SetNormalMagnet", collectableTime);
    }

    private void SetNormalMagnet()
    {
        SetMagnetRange(normalMagnetRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ExperienceParticle experiance))
        {
            MoveExperiance(experiance);
        }
    }

    private void MoveExperiance(ExperienceParticle experienceParticle)
    {
        /*if (!experienceParticle.isTweening)
        {
            experienceParticle.isTweening = true;
            Vector3 targetPosition = transform.position;
            float distance = Vector3.Distance(experienceParticle.transform.position, targetPosition);
            float time = distance / minPullSpeed;
            experienceParticle.transform.DOMove(
                targetPosition,
                time
            ).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                experienceParticle.isTweening = false;
                
                ObjectPooler.Instance.ReturnObject(experienceParticle.gameObject);

                GlobalGameEventManager.Instance.Notify("PlayerGetExperiance", experienceParticle.experience);
            });
        }*/
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}
