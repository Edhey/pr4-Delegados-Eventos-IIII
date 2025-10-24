using UnityEngine;

/// <summary>
/// Singleton manager for tracking and managing game score 
/// </summary>
public class GameScoreManager : MonoBehaviour {
  public static GameScoreManager Instance { get; private set; }

  private int currentPlayerScore = 0;
  private int currentEnemyScore = 0;

  public delegate void PlayerScoreChangedHandler(int newScore);
  public event PlayerScoreChangedHandler OnPlayerScoreChanged;
  public delegate void EnemyScoreChangedHandler(int newScore);
  public event EnemyScoreChangedHandler OnEnemyScoreChanged;


  private void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else {
      Destroy(gameObject);
    }
  }

  public void AddPlayerScore(int points) {
    currentPlayerScore += points;
    OnPlayerScoreChanged?.Invoke(currentPlayerScore);
    Debug.Log($"Player score updated: {currentPlayerScore}");
  }

  public void AddEnemyScore(int points) {
    currentEnemyScore += points;
    OnEnemyScoreChanged?.Invoke(currentEnemyScore);
    Debug.Log($"Enemy score updated: {currentEnemyScore}");
  }

  public void ResetScore() {
    currentPlayerScore = 0;
    currentEnemyScore = 0;
    OnPlayerScoreChanged?.Invoke(currentPlayerScore);
    OnEnemyScoreChanged?.Invoke(currentEnemyScore);
  }

  public int GetPlayerScore() {
    return currentPlayerScore;
  }

  public int GetEnemyScore() {
    return currentEnemyScore;
  }
}
