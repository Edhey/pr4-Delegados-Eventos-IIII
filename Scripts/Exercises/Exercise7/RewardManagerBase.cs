using UnityEngine;

/// <summary>
/// Exercise 7: Manages rewards based on score thresholds
/// </summary>
public class RewardManagerBase : MonoBehaviour {
  [SerializeField] private GameObject rewardPrefab;
  [SerializeField] private Transform spawnPoint;
  [SerializeField] private int scoreThreshold = 100;
  private int grantedRewards = 0;
  [SerializeField] private Vector3 teleportZoneCenter = new Vector3(20, 0, 20);
  [SerializeField] private Vector3 teleportZoneSize = new Vector3(10, 2, 10);
  [SerializeField] private GameObject footman;
  [SerializeField] private GameObject lich;
  private bool teleported = false;

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
    if (currentScore >= scoreThreshold) {
      if (!teleported) {
        TeleportAllBlueShields();
        teleported = true;
      }
      MoveFootmanToLich();
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

  private void TeleportAllBlueShields() {
    GameObject[] blueShields = GameObject.FindGameObjectsWithTag("Blue Shield");
    foreach (GameObject shield in blueShields) {
      MovementController shieldMovement = shield.GetComponent<MovementController>();
      shieldMovement.MoveTo(
        teleportZoneCenter + new Vector3(
          Random.Range(-teleportZoneSize.x / 2, teleportZoneSize.x / 2),
          Random.Range(-teleportZoneSize.y / 2, teleportZoneSize.y / 2),
          Random.Range(-teleportZoneSize.z / 2, teleportZoneSize.z / 2)
        )
      );
    }
  }

  private void MoveFootmanToLich() {
    if (footman != null && lich != null) {
      MovementController move = footman.GetComponent<MovementController>();
      if (move != null) {
        move.MoveTowards(lich.transform.position, 4f);
      }
    }
  }
}
