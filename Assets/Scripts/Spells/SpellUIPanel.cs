using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUIPanel : MonoBehaviour
{
    public Spell spell; // Büyü referansı

    public Image spellIcon; // Büyü simgesi
    public TextMeshProUGUI spellNameText; // Spell adı metni
    public TextMeshProUGUI descriptionText; // Açıklama metni
    public Button selectButton; // Seçim butonu

    public void OnEnable()
    {
        selectButton.onClick.AddListener(OnSelectButtonClicked); // Butona tıklama olayını ekle
    }

    public void OnDisable()
    {
        selectButton.onClick.RemoveListener(OnSelectButtonClicked); // Butondan tıklama olayını kaldır
    }
    public void SetSpell(Spell spell)
    {
        this.spell = spell; // Büyü referansını ayarla
        UpdateUI(); // UI'yi güncelle
    }

    private void UpdateUI()
    {
        if (spell == null)
        {
            Debug.LogError("Spell is null!");
            spellIcon.sprite = null; // Büyü simgesini temizle
            spellNameText.text = ""; // Büyü adını temizle
            descriptionText.text = ""; // Açıklama metnini temizle
            return;
        }

        spellIcon.sprite = spell.Icon; // Büyü simgesini ayarla
        spellNameText.text = spell.Name; // Büyü adını ayarla
        descriptionText.text = spell.Description; // Açıklama metnini ayarla
    }

    private void OnSelectButtonClicked()
    {
        // Büyü seçildiğinde yapılacak işlemler
        Debug.Log("Selected Spell: " + spell.Name);
        // Burada büyüyü seçme işlemini gerçekleştirebilirsiniz

        SpellManager.Instance.EquipSpell(spell); // Büyüyü ekipman olarak ekle
    }
}
