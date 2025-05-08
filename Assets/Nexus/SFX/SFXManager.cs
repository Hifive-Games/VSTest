using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SFX // add all your SFX here
{
    ExpShard,
    BulletFire,
    EnemyHit,
    EnemyDie,
    LevelUp,
    ArcaneMissile,
    Fireball,
    FireballExplosion,
    AcidBottle,
    AcidExplosion
}

[System.Serializable]
public class Sound
{
    public SFX id;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(.5f, 1.5f)] public float pitchMin = 1f;
    [Range(.5f, 1.5f)] public float pitchMax = 1f;
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [Tooltip("Configure all your sound clips here")]
    public List<Sound> sounds = new List<Sound>();

    [Tooltip("Number of pooled sources for concurrent SFX")]
    public int poolSize = 10;

    private Dictionary<SFX, Sound> _lookup;
    private Queue<AudioSource> _pool;
    private GameObject _sourcePrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // build lookup
            _lookup = sounds.ToDictionary(s => s.id, s => s);

            // create a hidden prefab to spawn sources from
            _sourcePrefab = new GameObject("SFX_Source");
            var a = _sourcePrefab.AddComponent<AudioSource>();
            a.playOnAwake = false;
            _sourcePrefab.SetActive(false);

            // warm up pool
            _pool = new Queue<AudioSource>();
            for (int i = 0; i < poolSize; i++)
                _pool.Enqueue(SpawnNew());
        }
        else Destroy(gameObject);
    }

    AudioSource SpawnNew()
    {
        var go = Instantiate(_sourcePrefab, transform);
        go.name = "PooledSource";
        go.SetActive(true);
        return go.GetComponent<AudioSource>();
    }

    AudioSource GetSource()
    {
        if (_pool.Count > 0) return _pool.Dequeue();
        return SpawnNew();
    }

    IEnumerator ReturnToPool(AudioSource src, float delay)
    {
        yield return new WaitForSeconds(delay);
        src.clip = null;
        _pool.Enqueue(src);
    }

    /// <summary>
    /// Play a 2D (UI) sound
    /// </summary>
    public void Play2D(SFX id)
    {
        if (!_lookup.TryGetValue(id, out var s)) return;
        var src = GetSource();
        src.spatialBlend = 0f;
        src.clip = s.clip;
        src.volume = s.volume;
        src.pitch = Random.Range(s.pitchMin, s.pitchMax);
        src.Play();
        StartCoroutine(ReturnToPool(src, s.clip.length / src.pitch));
    }

    /// <summary>
    /// Play a 3D sound at a world position
    /// </summary>
    public void PlayAt(SFX id, float spatialBlend = 0f)
    {
        if (!_lookup.TryGetValue(id, out var s)) return;
        var src = GetSource();
        src.transform.position = Vector3.zero;
        src.spatialBlend = Mathf.Clamp01(spatialBlend);
        src.clip = s.clip;
        src.volume = s.volume;
        src.pitch = Random.Range(s.pitchMin, s.pitchMax);
        src.Play();
        StartCoroutine(ReturnToPool(src, s.clip.length / src.pitch));
    }
}