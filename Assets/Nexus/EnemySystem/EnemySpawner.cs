using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Phase Data (ScriptableObjects)")]
    [SerializeField] private SpawnPhaseData[] phases;

    [Header("Gathering / Culling")]
    [SerializeField] private float gatheringDistance = 30f;
    [SerializeField] private float gatherCheckInterval = 5f;

    private Camera _cam;
    private Transform _player;
    private List<GameObject> _active = new List<GameObject>();

    private int _currentPhase = 0;
    private float _phaseTimer = 0f;
    private float _nextSpawnTime = 0f;
    private int _clustersThisPhase = 0;
    private float _nextGatherCheck = 0f;

    public TMPro.TMP_Text TimerText;
    public TMPro.TMP_Text enemyCountText;
    public TMPro.TMP_Text killsText;
    public TMPro.TMP_Text phaseText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _cam = Camera.main;
        _player = FindObjectOfType<CharacterController>().transform;
    }

    private void Update()
    {
        _phaseTimer += Time.deltaTime;

        if (_currentPhase < phases.Length)
        {
            var phase = phases[_currentPhase];

            // phase hasn't started yet?
            if (_phaseTimer < phase.startTime) return;

            // spawn clusters while within phase duration
            if (_phaseTimer <= phase.startTime + phase.duration)
            {
                if (Time.time >= _nextSpawnTime && _clustersThisPhase < phase.maxClusterGroups)
                {
                    SpawnCluster(phase);
                    _clustersThisPhase++;
                    _nextSpawnTime = Time.time + phase.spawnInterval;
                }
            }
            else
            {
                // end current phase
                EndPhase();
            }
        }

        // periodic gather / cull
        if (Time.time >= _nextGatherCheck)
        {
            GatherAndCull();
            _nextGatherCheck = Time.time + gatherCheckInterval;
        }

        // update UI
        UpdateUI();
    }

    public void UpdateUI()
    {
        // update timer text but min:sec format
        int minutes = (int)(_phaseTimer / 60f);
        int seconds = (int)(_phaseTimer % 60f);
        TimerText.text = $"{minutes:D2}:{seconds:D2}";
        enemyCountText.text = $"Enemies: {_active.Count}";

        // update phase text(phase name)
        if (_currentPhase < phases.Length)
        {
            phaseText.text = $"Phase: {phases[_currentPhase].name}";
        }
        else
        {
            phaseText.text = "Phase: None";
        }
    }

    public void AddKill()
    {
        killsText.text = $"Kills: {int.Parse(killsText.text.Split(':')[1]) + 1}";
    }

    private void SpawnCluster(SpawnPhaseData phase)
    {
        Vector3 center = GetOutsideCameraView();
        foreach (var grp in phase.enemyGroups)
        {
            for (int i = 0; i < grp.amount; i++)
            {
                Vector3 pos = center + Random.insideUnitSphere * grp.maxSpawnRadius;
                pos.y = 1.5f;

                // enforce min distance from player
                if (Vector3.Distance(pos, _player.position) < grp.minSpwanRadius)
                {
                    pos = center + (pos - center).normalized * grp.minSpwanRadius;
                    pos.y = 1.5f;
                }

                var go = EnemyFactory.CreateEnemy(grp.enemyData, pos);
                _active.Add(go);
            }
        }
    }

    private void EndPhase()
    {
        _currentPhase++;
        _phaseTimer = 0f;
        _clustersThisPhase = 0;
    }

    private void GatherAndCull()
    {
        // gather: if average out‐of‐bounds cluster too far, teleport them
        var oob = new List<GameObject>();
        Vector3 sum = Vector3.zero;
        foreach (var e in _active)
        {
            if (!IsInView(e.transform.position))
            {
                oob.Add(e);
                sum += e.transform.position;
            }
        }
        if (oob.Count > 0)
        {
            var avg = sum / oob.Count;
            if (Vector3.Distance(avg, _player.position) > gatheringDistance)
            {
                foreach (var e in oob)
                    e.transform.position = GetOutsideCameraView();
            }
        }

        // cull any dead/invisible if you want:
        _active.RemoveAll(e =>
        {
            if (e == null) return true;
            if (!IsInView(e.transform.position) && !e.TryGetComponent<BossActionSystem>(out _))
            {
                ObjectPooler.Instance.ReturnObject(e);
                return true;
            }
            return false;
        });
    }

    private Vector3 GetOutsideCameraView()
    {
        for (int i = 0; i < 20; i++)
        {
            var p = _player.position + Random.insideUnitSphere * 50f;
            p.y = 1.5f;
            if (!IsInView(p)) return p;
        }
        var fallback = _player.position + Random.insideUnitSphere * 70f;
        fallback.y = 1.5f;
        return fallback;
    }

    private bool IsInView(Vector3 pos)
    {
        var vp = _cam.WorldToViewportPoint(pos);
        const float off = .1f;
        return vp.x > -off && vp.x < 1 + off && vp.y > -off && vp.y < 1 + off;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (_active.Contains(enemy))
        {
            _active.Remove(enemy);
            ObjectPooler.Instance.ReturnObject(enemy);
        }
    }
    public void ClearEnemies()
    {
        foreach (var enemy in _active)
        {
            if (enemy != null) ObjectPooler.Instance.ReturnObject(enemy);
        }
        _active.Clear();
    }
}