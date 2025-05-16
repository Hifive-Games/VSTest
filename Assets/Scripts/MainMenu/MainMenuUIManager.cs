using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public int sceneIndex; // Yüklemek istediğiniz sahnenin indexi

    private void Start()
    {
        Application.targetFrameRate = 60; // Uygulamanın hedef kare hızını ayarla

        SFXManager.Instance.PickRandomMusic(); // Rastgele müzik çal
    }

    public void OpenStartGameScene()
    {
        Time.timeScale = 1; // Oyun zamanını başlat
        SFXManager.Instance.MuteMusic(); // Müzikleri sustur
        SceneManager.LoadScene(sceneIndex); // Belirtilen sahneyi yükle
    }

    public void ExitGame()
    {
        Debug.Log("Uygulama kapatıldı."); // Konsola mesaj yazdır
        Application.Quit(); // Uygulamayı kapat
    }
}
