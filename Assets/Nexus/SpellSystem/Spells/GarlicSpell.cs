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

        //check if the garlic is triggered by the audio spectrum, lerp back the radius to 5f if not triggered
        radius = math.lerp(math.max(AudioSpectrum.Instance.GetAmplitude() * 5f,5), 5f, Time.deltaTime/2f);
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
        transform.Rotate(Vector3.up, 360 * Time.deltaTime);
    }

    public Vector3 FollowCasterTransform()
    {
        Vector3 pos = TheHero.Instance.transform.position;
        pos.y = 0;

        return pos;
    }

    void DamageNearbyEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius / 2f);
        foreach (Collider c in colliders)
        {
            if (c.TryGetComponent(out Enemy e) && Caster == Caster.Player)
                e.TakeDamage(damage);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
    }
}