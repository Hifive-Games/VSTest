using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public int sceneIndex; // Yüklemek istediğiniz sahnenin indexi

    public void OpenStartGameScene()
    {
        SceneManager.LoadScene(sceneIndex); // Belirtilen sahneyi yükle
    }
}
