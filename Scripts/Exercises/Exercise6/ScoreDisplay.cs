using TMPro;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Exercise 6: Displays score in UI
/// </summary>
public class ScoreDisplay : MonoBehaviour {
  [SerializeField] private TMP_Text scoreText;

  private void OnEnable() {
    if (ScoreManager.Instance != null) {
      ScoreManager.Instance.OnScoreChanged += UpdateScoreDisplay;
      UpdateScoreDisplay(ScoreManager.Instance.GetScore());
    }
    else {
      Debug.LogWarning("ScoreManager instance not found.");
    }
  }

  private void OnDisable() {
    if (ScoreManager.Instance != null) {
      ScoreManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
    }
  }

  private void UpdateScoreDisplay(int newScore) {
    if (scoreText != null) {
      scoreText.text = $"Score: {newScore}";
    }
  }
}
