using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSpawner : MonoBehaviour
{
    [FormerlySerializedAs("playerCharacters")] [SerializeField] private HeroBaseData[] Heroes;  // Karakter SO'larını dizi olarak alıyoruz

    private void Awake()
    {
        // PlayerSO'ları Resources klasöründen dinamik olarak yüklüyoruz
        Heroes = Resources.LoadAll<HeroBaseData>(ResourcePathManager.Instance.GetHeroSOPath());

        HeroBaseData selectedHero = GetSelectedCharacter();

        if (selectedHero != null && selectedHero.isSelected)
        {
            Instantiate(selectedHero.prefab, Vector3.zero, Quaternion.identity); // Seçilen karakteri spawn et
            selectedHero.RunAllPassiveUpgrades();
        }
        else
        {
            Debug.LogError("Selected character not found or no character is selected.");
        }
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