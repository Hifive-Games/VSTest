using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private HeroBaseData[] playerCharacters;
    [SerializeField] private Image characterDisplayImage; // Karakter görselini gösterecek UI Image

    private int currentIndex = 0;

    private void Start()
    {
        GetPlayerCharacters();
        UpdateCharacterDisplay();
    }

    private void GetPlayerCharacters()
    {
       playerCharacters=Resources.LoadAll<HeroBaseData>(ResourcePathManager.Instance.GetHeroSOPath());
    }

    public void NextCharacter()
    {
        currentIndex = (currentIndex + 1) % playerCharacters.Length;
        UpdateCharacterDisplay();
    }

    public void PreviousCharacter()
    {
        currentIndex = (currentIndex - 1 + playerCharacters.Length) % playerCharacters.Length;
        UpdateCharacterDisplay();
    }

    private void UpdateCharacterDisplay()
    {
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            playerCharacters[i].isSelected = (i == currentIndex); // Sadece seçilen karakterin `isSelected` değeri true olacak
        }

        characterDisplayImage.sprite = playerCharacters[currentIndex].characterImage; // UI'da gösterilecek resmi güncelle
    }
}