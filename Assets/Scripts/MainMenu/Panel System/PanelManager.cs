using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }

    [SerializeField] private GameObject mainPanel; // Geri dönülemeyen ana panel
    private Stack<GameObject> panelStack = new Stack<GameObject>();
    private GameObject currentPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        OpenPanel(mainPanel); // Başlangıçta ana paneli aç
    }

    public void OpenPanel(GameObject panel)
    {
        // Mevcut paneli yığın içerisine ekle
        if (currentPanel != null)
        {
            panelStack.Push(currentPanel); // Mevcut paneli yığına ekle
            currentPanel.SetActive(false);  // Mevcut paneli kapat
        }

        // Yeni paneli etkinleştir
        currentPanel = panel;
        currentPanel.SetActive(true); // Yeni paneli aç
    }

    public void GoBack()
    {
        // Ana panelde geri gitmeye çalışılmasını önlemek için yığının boşluğunu kontrol et
        if (panelStack.Count > 0)
        {
            currentPanel.SetActive(false); // Şu anki paneli kapat
            currentPanel = panelStack.Pop(); // Yığından önceki paneli al
            currentPanel.SetActive(true); // Önceki paneli aç
        }
        else
        {
            Debug.LogWarning("Ana panele geri dönülemez.");
        }
    }
}