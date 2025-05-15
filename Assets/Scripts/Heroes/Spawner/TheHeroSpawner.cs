using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class TheHeroSpawner : MonoBehaviour
{
    [SerializeField] private HeroBaseData[] Heroes;  // Karakter SO'larını dizi olarak alıyoruz
    private HeroBaseData selectedHero;

    //[SerializeField] private GameObject pm;
    private void Awake()
    {
        Application.targetFrameRate = 120; // Uygulamanın hedef kare hızını ayarla
        InstantianteTheHero();
    }

    private void InstantianteTheHero()
    {
        // PlayerSO'ları Resources klasöründen dinamik olarak yüklüyoruz
        Heroes = Resources.LoadAll<HeroBaseData>(ResourcePathManager.Instance.GetHeroSOPath());

        selectedHero = GetSelectedCharacter();

        if (selectedHero != null && selectedHero.isSelected)
        {
            Instantiate(selectedHero.prefab, Vector3.zero, Quaternion.identity); // Seçilen karakteri spawn et
            selectedHero.RunAllHeroStats();
            selectedHero.RunAllPassiveUpgrades();
        }
        else
        {
            Debug.LogError("Selected character not found or no character is selected.");
        }

        //Instantiate(pm, TheHero.Instance.transform); // Player Magnet'i spawn et
    }

    // Seçili karakteri bulma
    private HeroBaseData GetSelectedCharacter()
    {
        foreach (var character in Heroes)
        {
            if (character.isSelected)
            {
                return character;
            }
        }
        return null; // Hiçbir karakter seçilmemişse null döndürüyoruz
    }


}