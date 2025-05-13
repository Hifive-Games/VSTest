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
    }

    public void OpenStartGameScene()
    {
        Time.timeScale = 1; // Oyun zamanını başlat
        SceneManager.LoadScene(sceneIndex); // Belirtilen sahneyi yükle
    }
}
