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

    [Header("Bosses")]
    [SerializeField] private Bosses[] bosses;

    private bool _bossActive = false;
    private GameObject _currentBoss;
    private int _nextBossIndex = 0;

    [Header("Timer")]
    [SerializeField] private float timer = 0f;

    private Camera _cam;
    private Transform _player;
    [SerializeField] private List<GameObject> _active = new List<GameObject>();

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
        UpdateUI();

        // culling/gathering unchanged…
        if (Time.time >= _nextGatherCheck)
        {
            GatherEnemies();
            _nextGatherCheck = Time.time + gatherCheckInterval;
        }

        // 1) If boss is up, only watch for its death
        if (_bossActive)
        {
            if (_currentBoss == null || !_currentBoss.activeInHierarchy)
                OnBossDefeated();
            else
                return;
        }

        // 2) Try to spawn next boss at its scheduled game‐time
        TrySpawnBosses();

        // 3) Normal phase logic – countdown ONLY when no boss is active
        if (_currentPhase < phases.Length)
        {
            var phase = phases[_currentPhase];
            if (_phaseTimer < phase.startTime) return;

            if (_phaseTimer <= phase.startTime + phase.duration)
            {
                switch (phase.behavior)
                {
                    case SpawnBehavior.Cluster: TrySpawnCluster(phase); break;
                    case SpawnBehavior.MaintainCount: TryMaintainCount(phase); break;
                }
            }
            else
            {
                EndPhase();
            }
        }
    }

    private void TrySpawnBosses()
    {
        if (_bossActive || _nextBossIndex >= bosses.Length) return;

        var bossData = bosses[_nextBossIndex];
        if (Time.time >= bossData.spawnTime)
        {
            Vector3 pos = _player.position + Random.insideUnitSphere * bossData.spawnDistance;
            pos.y = 1.5f;
            _currentBoss = ObjectPooler.Instance.SpawnFromPool(bossData.prefab, pos, Quaternion.identity);
            _currentBoss.transform.position = new Vector3(pos.x, 2f, pos.z);
            _currentBoss.transform.LookAt(_player.position);
            _bossActive = true;
            _nextBossIndex++;
        }
    }
    private void OnBossDefeated()
    {
        // simply un‐pause.  We do NOT adjust _currentPhase or _phaseTimer –
        // any phases whose windows expired during the fight are gone.
        _bossActive = false;
    }

    public void UpdateUI()
    {
        // update timer text but min:sec format
        int minutes = (int)(Time.time / 60f);
        int seconds = (int)(Time.time % 60f);
        timer = Time.time;
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
    private void TrySpawnCluster(SpawnPhaseData phase)
    {
        if (Time.time >= _nextSpawnTime
            && _clustersThisPhase < phase.maxClusterGroups)
        {
            SpawnCluster(phase);
            _clustersThisPhase++;
            _nextSpawnTime = Time.time + phase.spawnInterval;
        }
    }

    private void TryMaintainCount(SpawnPhaseData phase)
    {
        if (Time.time < _nextSpawnTime) return;

        int current = _active.Count;
        int threshold = Mathf.FloorToInt(phase.targetEnemyCount * (1f - phase.refillThreshold));
        if (current < threshold)
        {
            int missing = phase.targetEnemyCount - current;
            SpawnMaintain(phase, missing);
        }

        _nextSpawnTime = Time.time + phase.maintainSpawnInterval;
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
    private void SpawnMaintain(SpawnPhaseData phase, int missing)
    {
        for (int i = 0; i < missing; i++)
        {
            // pick a random group to spawn one from
            var grp = phase.enemyGroups[Random.Range(0, phase.enemyGroups.Length)];
            Vector3 pos = GetSpawnPosition(grp);
            var go = EnemyFactory.CreateEnemy(grp.enemyData, pos);
            _active.Add(go);
        }
    }

    private Vector3 GetSpawnPosition(EnemySpawnGroup grp)
    {
        var center = GetOutsideCameraView();
        Vector3 pos = center + Random.insideUnitSphere * grp.maxSpawnRadius;
        pos.y = 1.5f;

        float dist = Vector3.Distance(pos, _player.position);
        if (dist < grp.minSpwanRadius)
        {
            pos = center + (pos - center).normalized * grp.minSpwanRadius;
            pos.y = 1.5f;
        }

        return pos;
    }

    private void EndPhase()
    {
        _currentPhase++;
        _phaseTimer = 0f;
        _clustersThisPhase = 0;
    }

    private void GatherEnemies()
    {
        foreach (var enemy in _active)
        {
            if (enemy == null) continue;
            if (Vector3.Distance(enemy.transform.position, _player.position) > gatheringDistance)
            {
                Vector3 pos = GetOutsideCameraView();
                enemy.transform.position = pos;
            }
        }

    }

    private Vector3 GetOutsideCameraView()
    {
        float spawnHeight = 1.5f;

        float minR = gatheringDistance;
        float maxR = gatheringDistance * 1.5f;

        // try a bunch of random angles/radii until it’s outside camera
        for (int i = 0; i < 20; i++)
        {
            float ang = Random.Range(0f, Mathf.PI * 2f);
            float r = Random.Range(minR, maxR);
            Vector3 dir = new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang));
            Vector3 p = _player.position + dir * r;
            p.y = spawnHeight;
            if (!IsInView(p)) return p;
        }

        // fallback: just pick any direction at a larger radius
        float fallbackAng = Random.Range(0f, Mathf.PI * 2f);
        Vector3 fd = new Vector3(Mathf.Cos(fallbackAng), 0f, Mathf.Sin(fallbackAng));
        Vector3 fallback = _player.position + fd * (maxR * 2f);
        fallback.y = spawnHeight;
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
        // just remove from active list, don’t pool here
        if (_active.Remove(enemy))
        {
            _clustersThisPhase = Mathf.Max(0, _clustersThisPhase - 1);
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
    public void SpawnEnemy(EnemyDataSO enemyData, Vector3 position)
    {
        var go = EnemyFactory.CreateEnemy(enemyData, position);
        _active.Add(go);
    }
}

[System.Serializable]
public struct Bosses
{
    public GameObject prefab;
    public float spawnTime;
    public float spawnDistance;
}