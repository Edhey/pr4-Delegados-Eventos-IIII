using TMPro;

using UnityEngine;

/// <summary>
/// Exercise 8: Displays player and enemy score in UI
/// </summary>
public class GameScoreDisplay : MonoBehaviour {
  [SerializeField] private TMP_Text playerScoreText;
  [SerializeField] private TMP_Text enemyScoreText;

  private void OnEnable() {
    if (GameScoreManager.Instance != null) {
      GameScoreManager.Instance.OnPlayerScoreChanged += UpdatePlayerScoreDisplay;
      GameScoreManager.Instance.OnEnemyScoreChanged += UpdateEnemyScoreDisplay;
      UpdatePlayerScoreDisplay(GameScoreManager.Instance.GetPlayerScore());
      UpdateEnemyScoreDisplay(GameScoreManager.Instance.GetEnemyScore());
    }
    else {
      Debug.LogWarning("GameScoreManager instance not found.");
    }
  }

  private void OnDisable() {
    if (GameScoreManager.Instance != null) {
      GameScoreManager.Instance.OnPlayerScoreChanged -= UpdatePlayerScoreDisplay;
      GameScoreManager.Instance.OnEnemyScoreChanged -= UpdateEnemyScoreDisplay;
    }
  }

  private void UpdatePlayerScoreDisplay(int newScore) {
    if (playerScoreText != null) {
      playerScoreText.text = $"Player Score: {newScore}";
    }
  }

  private void UpdateEnemyScoreDisplay(int newScore) {
    if (enemyScoreText != null) {
      enemyScoreText.text = $"Enemy Score: {newScore}";
    }
  }
}
