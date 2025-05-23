using Unity.Mathematics;
using UnityEngine;

public class GarlicSpell : Spell
{
    #region Unity Event Hooks
    public override void OnEnable() => AudioSpectrum.OnBandTrigger += OnBandTrigger;
    public override void OnDisable() => AudioSpectrum.OnBandTrigger -= OnBandTrigger;
    private void OnDestroy() => AudioSpectrum.OnBandTrigger -= OnBandTrigger;
    #endregion

    float tickTimer;

    private bool[] bandTriggered = new bool[8];

    private Collider[] _hitBuffer = new Collider[16]; // Reusable buffer for overlap sphere

    public override void Release()
    {
        StopAllCoroutines();
    }

    public override void Seek(Transform target = null)
    {
        //no need to seek, garlic will follow the player
    }

    #region Event Handlers
    private void OnBandTrigger(int band)
    {
        if (band >= 0 && band < bandTriggered.Length)
            bandTriggered[band] = true;
    }
    #endregion

    private void Update()
    {
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0)
        {
            DamageNearbyEnemies();
            tickTimer = tickInterval;
        }

        if(SFXManager.Instance.muteAll)
        {
            for (int i = 0; i < bandTriggered.Length; i++)
            {
                if (bandTriggered[i])
                {
                    radius = Mathf.Lerp(radius, AudioSpectrum.Instance._maxAmplitude * 10f, Time.deltaTime * 2f);
                    bandTriggered[i] = false;
                }
                else
                {
                    radius = Mathf.Lerp(radius, 5f, Time.deltaTime * 2f);
                }
            }
        }

        gameObject.transform.localScale = new Vector3(radius, .1f, radius);


        //return the garlic when duration is over
        if (duration > 0)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                ObjectPooler.Instance.ReturnObject(gameObject);
            }
        }


        transform.position = FollowCasterTransform();

        //rotate the garlic
        transform.Rotate(Vector3.up, 360 * Time.deltaTime * 0.5f);
    }

    public Vector3 FollowCasterTransform()
    {
        Vector3 pos = TheHero.Instance.transform.position;
        pos.y = 0;

        return pos;
    }

    void DamageNearbyEnemies()
    {
        int hits = Physics.OverlapSphereNonAlloc(transform.position, radius / 2, _hitBuffer);
        Collider[] colliders = new Collider[hits];
        for (int i = 0; i < hits; i++)
        {
            colliders[i] = _hitBuffer[i];
        }

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                // Apply damage
                enemy.TakeDamage(damage, DamageNumberType.Spell);
            }
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
    }
}