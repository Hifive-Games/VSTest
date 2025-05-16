using System;
using System.Collections.Generic;
using UnityEngine;

public class TheHeroExperienceManager : MonoBehaviourSingleton<TheHeroExperienceManager>
{
    private GameObject experienceColliderObject;
    private SphereCollider sphereCollider;

    [SerializeField] private float initialRadius = 1f;
    [SerializeField] private HeroExperienceLevelSO experienceData;

    private int currentLevel = 1;
    private int currentXP = 0;

    // Çoklu level atlama durumunu kontrol eden Stack
    private Stack<int> levelUpStack = new Stack<int>();
    private bool isLevelUpInProgress = false;

    private void OnEnable()
    {
        GameEvents.OnExperienceGathered += ExperienceGathered;
        GameEvents.OnGameResumed += ProcessNextLevelInStack;
    }

    private void OnDisable()
    {
        GameEvents.OnExperienceGathered -= ExperienceGathered;
        GameEvents.OnGameResumed -= ProcessNextLevelInStack;
    }

    private void Start()
    {
        experienceColliderObject = new GameObject("ExperienceCollider");
        experienceColliderObject.transform.SetParent(transform);
        experienceColliderObject.transform.localPosition = Vector3.zero;
        experienceColliderObject.layer = LayerMask.NameToLayer("Colliders");

        experienceColliderObject.AddComponent<ExperienceCollider>();

        sphereCollider = experienceColliderObject.AddComponent<SphereCollider>();
        sphereCollider.radius = initialRadius;
        sphereCollider.isTrigger = true;

        Rigidbody rb = experienceColliderObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void AddExperienceSize(float a)
    {
        if (sphereCollider != null)
        {
            sphereCollider.radius *= (1 + a / 100f);
        }
    }

    public void ReduceExperienceSize(float a)
    {
        if (sphereCollider != null)
        {
            sphereCollider.radius *= (1 - a / 100f);

            if (sphereCollider.radius < 0.5f)
            {
                sphereCollider.radius = 0.5f;
            }
        }
    }

    private void ExperienceGathered(int xp)
    {
        if (experienceData == null || experienceData.experienceRequired.Length == 0)
        {
            Debug.LogError("Experience data is missing!");
            return;
        }

        currentXP += xp;

        int maxLevelIndex = experienceData.experienceRequired.Length - 1;
        int requiredXP = currentLevel - 1 <= maxLevelIndex
            ? experienceData.experienceRequired[currentLevel - 1]
            : experienceData.experienceRequired[maxLevelIndex];

        GameEvents.OnExperienceUpdated?.Invoke(currentXP, requiredXP);

        while (currentXP >= requiredXP)
        {
            currentXP -= requiredXP;
            currentLevel++;
            levelUpStack.Push(currentLevel);

            requiredXP = currentLevel - 1 <= maxLevelIndex
                ? experienceData.experienceRequired[currentLevel - 1]
                : experienceData.experienceRequired[maxLevelIndex];

            GameEvents.OnExperienceUpdated?.Invoke(currentXP, requiredXP);
        }

        if (!isLevelUpInProgress && levelUpStack.Count > 0)
        {
            isLevelUpInProgress = true;
            ProcessNextLevelInStack();
        }
    }

    private void ProcessNextLevelInStack()
    {
        if (levelUpStack.Count > 0)
        {
            int level = levelUpStack.Pop();
            Debug.Log($"Level Up to: {level}");
            GameEvents.OnLevelUp?.Invoke();
        }

        if (levelUpStack.Count == 0)
        {
            isLevelUpInProgress = false;
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}