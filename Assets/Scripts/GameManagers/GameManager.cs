using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private SpellManager spellManager;
    private Player player;

    private void Start()
    {
        // Initialize managers
        spellManager = SpellManager.Instance;
        player = Player.Instance;
    }
}