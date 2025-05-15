using UnityEngine;
using System.Collections.Generic;
using System;

public class SpellSelectionManager : MonoBehaviourSingleton<SpellSelectionManager>
{
    //this panel populate spells on the UI
    public GameObject MainPanel; // Ana panel
    public GameObject spellUIPanelPrefab; // Büyü UI paneli prefab'ı
    public Transform spellUIPanelParent; // Büyü UI panelinin ebeveyni
    public List<Spell> availableSpells; // Mevcut büyüler listesi
    public int maxSelectedSpells = 3; // Maksimum seçilen büyü sayısı

    public Action OnChestOpened; // Sandık açıldığında çağrılacak olay

    public void ChestOpened()
    {
        // Sandık açıldığında çağrılacak olayları tetikle
        OnChestOpened?.Invoke(); // Eğer olay varsa çağır
        MainPanel.SetActive(true); // Büyü seçim panelini kapat
        SetAvailableSpells(); // Mevcut büyüleri ayarla

        Time.timeScale = 0f; // Oyun zamanını durdur
    }

    public void SetAvailableSpells()
    {
        availableSpells = SpellDatabase.Instance.Spells; // Büyü veritabanından mevcut büyüleri al

        maxSelectedSpells = Mathf.Min(maxSelectedSpells, availableSpells.Count); // Maksimum seçilen büyü sayısını mevcut büyü sayısına göre ayarla

        //pick random spells from the available spells but not the same ones
        List<Spell> randomSpells = new List<Spell>();
        HashSet<int> selectedIndices = new HashSet<int>(); // Seçilen indeksleri saklamak için bir küme

        while (randomSpells.Count < maxSelectedSpells && randomSpells.Count < availableSpells.Count)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableSpells.Count); // Rastgele bir indeks seç
            if (!selectedIndices.Contains(randomIndex)) // Eğer bu indeks daha önce seçilmediyse
            {
                selectedIndices.Add(randomIndex); // İndeksi ekle
                randomSpells.Add(availableSpells[randomIndex]); // Rastgele büyüyü ekle
            }
        }

        // Clear the UI before populating new spells
        foreach (Transform child in spellUIPanelParent)
        {
            Destroy(child.gameObject); // Önceki büyü UI panellerini temizle
        }

        if (randomSpells.Count == 0)
        {
            Debug.LogError("No spells available to select from! - Giving Gold..."); // Seçilecek büyü yoksa hata mesajı ver
            MainPanel.SetActive(false); // Büyü seçim panelini kapat
            return;
        }

        if (SFXManager.Instance.muteAll)
        {
            {
                bool isSpell1Or4Present = false;
                foreach (Spell spell in randomSpells)
                {
                    if (spell.SpellID == 1 || spell.SpellID == 4)
                    {
                        // Eğer varsa, büyü sesini kapat
                        isSpell1Or4Present = true;
                    }
                    else
                    {
                        // Eğer yoksa, büyü sesini aç
                        isSpell1Or4Present = false;
                    }
                }

                if (!isSpell1Or4Present)
                {
                    randomSpells.RemoveAt(0); // Eğer 1 veya 4 yoksa, ilk büyüyü kaldır
                    //1 ya da 4 numaralı büyüyü ekle
                    int randomIndex = UnityEngine.Random.Range(0, 2); // 0 veya 1
                    if (randomIndex == 0)
                    {
                        randomSpells.Add(SpellDatabase.Instance.GetSpell(1)); // 1 numaralı büyüyü ekle
                    }
                    else
                    {
                        randomSpells.Add(SpellDatabase.Instance.GetSpell(4)); // 4 numaralı büyüyü ekle
                    }

                    Debug.Log("1 or 4 added to the selected spells."); // 1 veya 4 eklendi mesajı ver
                }
                else
                {
                    Debug.Log("1 or 4 is already present in the selected spells."); // 1 veya 4 zaten mevcutsa mesaj ver
                }
            }
        }


        // Populate the UI with the selected spells
        foreach (Spell spell in randomSpells)
        {
            GameObject spellUIPanelObject = Instantiate(spellUIPanelPrefab, spellUIPanelParent); // Yeni büyü UI paneli oluştur
            SpellUIPanel spellUIPanel = spellUIPanelObject.GetComponent<SpellUIPanel>(); // Büyü UI panel bileşenini al
            spellUIPanel.SetSpell(spell); // Büyü UI panelini ayarla
            spellUIPanel.selectButton.onClick.AddListener(() => OnSpellSelected(spell)); // Büyü seçildiğinde çağrılacak fonksiyonu ayarla
        }
    }

    private void OnSpellSelected(Spell spell)
    {
        // Büyüyü seçilen büyüler listesine ekle
        Debug.Log("Selected Spell: " + spell.Name); // Seçilen büyüyü konsola yazdır

        SpellDatabase.Instance.RemoveSpell(spell); // Büyüyü veritabanından çıkar

        MainPanel.SetActive(false); // Büyü seçim panelini kapat
        Time.timeScale = 1f; // Oyun zamanını başlat
    }
}
