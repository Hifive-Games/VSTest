using System;
using UnityEngine;

public class TheHeroExperienceManager : MonoBehaviour
{
    private GameObject experienceColliderObject; // Alt obje referansı
    private SphereCollider sphereCollider;
    
    [SerializeField] private float initialRadius = 1f;
    [SerializeField] private HeroExperienceLevelSO experienceData; // SO dosyası
    
    private int currentLevel = 1; // Oyuncunun mevcut seviyesi
    private int currentXP = 0; // Toplanan XP
    
    private void OnEnable()
    {
        GameEvents.OnExperienceGathered += ExperienceGathered;
    }
    
    private void OnDisable()
    {
        GameEvents.OnExperienceGathered -= ExperienceGathered;
    }

    private void Start()
    {
        // Alt obje oluştur ve ana karaktere bağla
        experienceColliderObject = new GameObject("ExperienceCollider");
        experienceColliderObject.transform.SetParent(transform);
        experienceColliderObject.transform.localPosition = Vector3.zero;

        experienceColliderObject.layer = LayerMask.NameToLayer("Colliders");

        
        // ExperienceCollider sınıfını da ekle
        experienceColliderObject.AddComponent<ExperienceCollider>();
        
        // Collider ekle
        sphereCollider = experienceColliderObject.AddComponent<SphereCollider>();
        sphereCollider.radius = initialRadius;
        sphereCollider.isTrigger = true;

        // Rigidbody ekle (Unity'nin trigger sisteminde sağlıklı çalışması için)
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
            requiredXP = currentLevel - 1 <= maxLevelIndex 
                ? experienceData.experienceRequired[currentLevel - 1] 
                : experienceData.experienceRequired[maxLevelIndex];
            
            GameEvents.OnExperienceUpdated?.Invoke(currentXP, requiredXP); // Yeni level için tekrar gönder
            GameEvents.OnLevelUp?.Invoke();
        }
    }


}
