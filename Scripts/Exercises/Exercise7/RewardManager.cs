using UnityEngine;

/// <summary>
/// Exercise 7: Manages rewards based on score thresholds
/// </summary>
public class RewardManager : MonoBehaviour {
  [SerializeField] private GameObject rewardPrefab;
  [SerializeField] private Transform spawnPoint;
  [SerializeField] private int scoreThreshold = 100;
  private int grantedRewards = 0;

  private void OnEnable() {
    if (ScoreManager.Instance != null) {
      ScoreManager.Instance.OnScoreChanged += CheckReward;
    }
    else {
      Debug.LogWarning("ScoreManager instance not found.");
    }
  }

  private void OnDisable() {
    if (ScoreManager.Instance != null) {
      ScoreManager.Instance.OnScoreChanged -= CheckReward;
    }
  }

  private void CheckReward(int currentScore) {
    int rewardsToGrant = currentScore / scoreThreshold;
    while (rewardsToGrant > grantedRewards) {
      GrantReward();
      grantedRewards++;
    }
  }

  private void GrantReward() {
    if (rewardPrefab != null && spawnPoint != null) {
      Instantiate(rewardPrefab, spawnPoint.position, Quaternion.identity);
      Debug.Log("Reward granted!");
    }
    else {
      Debug.LogWarning("RewardPrefab or SpawnPoint is not assigned.");
    }
  }
}
