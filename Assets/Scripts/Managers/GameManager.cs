using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    private bool isGamePaused = false;

    private void OnEnable()
    {
        GameEvents.OnGamePaused += PauseGame;
        GameEvents.OnZeroHealth += PauseGame;
        GameEvents.OnGameResumed += ResumeGame;
    }

    private void OnDisable()
    {
        GameEvents.OnGamePaused -= PauseGame;
        GameEvents.OnGameResumed -= ResumeGame;
        GameEvents.OnZeroHealth -= PauseGame;

    }

    private void PauseGame()
    {
        if (isGamePaused) return; // Zaten durduysa tekrar durdurma
        Time.timeScale = 0f;
        isGamePaused = true;
        Debug.Log("Oyun durdu!");
    }

    private void ResumeGame()
    {
        if (!isGamePaused) return; // Zaten çalışıyorsa tekrar başlatma
        Time.timeScale = 1;
        isGamePaused = false;
        Debug.Log("Oyun devam ediyor!");
    }
}