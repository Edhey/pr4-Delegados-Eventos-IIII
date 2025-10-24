using UnityEngine;

/// <summary>
/// Singleton manager for tracking and managing game score
/// </summary>
public class ScoreManager : MonoBehaviour {
  public static ScoreManager Instance { get; private set; }

  private int currentScore = 0;

  public delegate void ScoreChangedHandler(int newScore);
  public event ScoreChangedHandler OnScoreChanged;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else {
      Destroy(gameObject);
    }
  }

  public void AddScore(int points) {
    currentScore += points;
    OnScoreChanged?.Invoke(currentScore);
    Debug.Log($"Score updated: {currentScore}");
  }

  public void ResetScore() {
    currentScore = 0;
    OnScoreChanged?.Invoke(currentScore);
  }

  public int GetScore() {
    return currentScore;
  }
}
