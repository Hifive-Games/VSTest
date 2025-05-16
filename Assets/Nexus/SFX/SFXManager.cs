using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

[System.Serializable]
public class Musics
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}

public class SFXManager : MonoBehaviourSingletonPersistent<SFXManager>
{
    [Header("SFX Manager")]
    [Tooltip("Mute all sounds")]
    public bool muteAll = false;

    [Tooltip("Mute all sounds except UI")]
    public bool muteSFX = false;

    [Tooltip("Mute all UI sounds")]
    public bool muteUI = false;

    [Tooltip("Configure all your sound clips here")]
    public List<Sound> sounds = new List<Sound>();

    [Header("Music")]
    [Tooltip("Configure all your music clips here")]
    public List<Musics> musics = new List<Musics>();
    [Tooltip("Mute all music")]
    public bool muteMusic = false;

    private AudioSource _musicSource;

    [Tooltip("Number of pooled sources for concurrent SFX")]
    public int poolSize = 10;

    private Dictionary<SFX, Sound> _lookup;
    private Queue<AudioSource> _pool;
    private GameObject _sourcePrefab;

    void Start()
    {
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

        // pick a random music
        if (musics.Count > 0)
        {
            PickRandomMusic();
        }
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
        if (muteAll || muteSFX) return;

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

    //add a method to mute all sounds
    public void MuteAll(Toggle mute)
    {
        muteAll = mute.isOn;
        if (muteAll)
        {
            foreach (var sound in sounds)
            {
                sound.volume = 0f;
            }
            LerpMusicVolume(0f, 2f);
        }
        else
        {
            foreach (var sound in sounds)
            {
                sound.volume = 1f;
            }

            foreach (var music in musics)
            {
                music.volume = 1f;
            }

            LerpMusicVolume(1f, 2f);
        }
    }

    //lerp music volume
    public void LerpMusicVolume(float targetVolume, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(LerpMusicVolumeCoroutine(targetVolume, duration));
    }
    private IEnumerator LerpMusicVolumeCoroutine(float targetVolume, float duration)
    {
        if (_musicSource == null) yield break;

        float startVolume = _musicSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        _musicSource.volume = targetVolume;
    }

    public void PickRandomMusic()
    {
        if (muteAll || muteMusic) return;

        var music = musics[Random.Range(0, musics.Count)];
        var src = _musicSource;
        if (src == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            src = _musicSource;
        }
        src.clip = music.clip;
        src.volume = 0f;
        src.loop = true;
        src.Play();
        LerpMusicVolume(1, 5f);
    }

    //mute music when changing scenes
    public void MuteMusic()
    {
        _musicSource.volume = 0f;
        _musicSource.Stop();
    }
}