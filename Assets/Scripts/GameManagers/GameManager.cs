using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;
    public int score = 0;
    public bool isGameOver = false;

    public GameState gameState;
    public PlayerState playerState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //We will call this function to start the game
    public void StartGame()
    {
        gameState = GameState.InGame;
        playerState = PlayerState.Alive;
        score = 0;
        isGameOver = false;
        InterfaceManager.Instance.HideGameOverUI();
        InterfaceManager.Instance.HideFailedUI();
        InterfaceManager.Instance.HideLevelUpUI();
    }

    //We will call this function when the player dies
    public void GameOver()
    {
        gameState = GameState.Win;
        playerState = PlayerState.Dead;
        isGameOver = true;
        InterfaceManager.Instance.ShowGameOverUI();
    }

    //We will call this function when the player levels up
    public void LevelUp()
    {
        if (playerState == PlayerState.Alive)
        {
            gameState = GameState.InGame;
            playerState = PlayerState.LevelUp;
            InterfaceManager.Instance.ShowLevelUpUI();
        
            Time.timeScale = 0f;
        }
    }

    //We will call this function when the player fails
    public void Failed()
    {
        gameState = GameState.Failed;
        playerState = PlayerState.Dead;
        isGameOver = true;
        InterfaceManager.Instance.ShowFailedUI();
    }

    public void HandleGameState()
    {
        if (gameState == GameState.InGame)
        {
            if (playerState == PlayerState.Dead)
            {
                GameOver();
            }
            else if (playerState == PlayerState.LevelUp)
            {
                LevelUp();
            }
        }
        else if (gameState == GameState.Failed)
        {
            Failed();
        }
    }
}
