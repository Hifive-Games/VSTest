using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerSO[] playerCharacters;  // Karakter SO'larını dizi olarak alıyoruz

    private void Start()
    {
        // PlayerSO'ları Resources klasöründen dinamik olarak yüklüyoruz
        playerCharacters = Resources.LoadAll<PlayerSO>(ResourcePathManager.Instance.GetPlayerSOPath());

        PlayerSO selectedCharacter = GetSelectedCharacter();

        if (selectedCharacter != null && selectedCharacter.isSelected)
        {
            Instantiate(selectedCharacter.prefab, Vector3.zero, Quaternion.identity); // Seçilen karakteri spawn et
        }
        else
        {
            Debug.LogError("Selected character not found or no character is selected.");
        }
    }

    // Seçili karakteri bulma
    private PlayerSO GetSelectedCharacter()
    {
        foreach (var character in playerCharacters)
        {
            if (character.isSelected)
            {
                return character;
            }
        }
        return null; // Hiçbir karakter seçilmemişse null döndürüyoruz
    }
}