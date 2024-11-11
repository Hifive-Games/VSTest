using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviourSingleton<PanelManager>
{
    [SerializeField] private PanelController mainPanel; // Geri dönülemeyen ana panel
    private Stack<PanelController> panelStack = new Stack<PanelController>();
    private PanelController currentPanel;

    private void Start()
    {
        OpenPanel(mainPanel); // Başlangıçta ana paneli aç
    }

    public void OpenPanel(PanelController panel)
    {
        // Mevcut paneli yığın içerisine ekle
        if (currentPanel != null)
        {
            panelStack.Push(currentPanel); // Mevcut paneli yığına ekle
            currentPanel.OpenPanel();  // Mevcut paneli kapat
        }

        // Yeni paneli etkinleştir
        currentPanel = panel;
        currentPanel.OpenPanel(); // Yeni paneli aç
    }

    public void GoBack()
    {
        // Ana panelde geri gitmeye çalışılmasını önlemek için yığının boşluğunu kontrol et
        if (panelStack.Count > 0)
        {
            currentPanel.ClosePanel(); // Şu anki paneli kapat
            currentPanel = panelStack.Pop(); // Yığından önceki paneli al
            currentPanel.OpenPanel(); // Önceki paneli aç
        }
        else
        {
            Debug.LogWarning("Ana panele geri dönülemez.");
        }
    }
}