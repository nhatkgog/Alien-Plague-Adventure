using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Playing;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log("Game State Changed to: " + newState);
    }

    public bool IsPaused()
    {
        return CurrentState == GameState.Paused;
    }

    public bool IsVictory()
    {
        return CurrentState == GameState.Victory;
    }

    public bool IsGameOver()
    {
        return CurrentState == GameState.GameOver;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum GameState
{
    Playing,
    Paused,
    Victory,
    GameOver
}
