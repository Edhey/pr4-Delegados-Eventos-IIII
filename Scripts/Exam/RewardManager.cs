using UnityEngine;

/// <summary>
/// Exercise 7: Manages rewards based on score thresholds
/// </summary>
public class RewardManager : MonoBehaviour {
  [SerializeField] private int scoreThreshold = 100;
  [SerializeField] private ShieldCollector shieldCollector;
  [SerializeField] private float purpleShieldScaleMultiplier = 2f;
  private bool upThreshold = false;
  [SerializeField] private int alignThreshold = 200;
  private int alignDistance = 10;
  [SerializeField] private GameObject alignTargetPoint;
  private bool aligned = false;

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
    if (currentScore >= scoreThreshold) {
      if (!upThreshold) {
        shieldCollector.changePointsPerType1Shield(shieldCollector.pointsPerType1Shield * 2);
        ScaleAllPurpleShields();
        upThreshold = true;
      }
    }
    if (currentScore >= alignThreshold) {
      if (!aligned) {
        AlignBlueShields(alignTargetPoint.transform.position);
      }
    }
  }

  private void ScaleAllPurpleShields() {
    GameObject[] purpleShields = GameObject.FindGameObjectsWithTag("Purple Shield");
    foreach (GameObject shield in purpleShields) {
      shield.transform.localScale *= purpleShieldScaleMultiplier;
    }
  }

  private void AlignBlueShields(Vector3 targetPosition) {
    GameObject[] blueShields = GameObject.FindGameObjectsWithTag("Blue Shield");
    Vector3 forward = alignTargetPoint.transform.forward;
    int index = 0;
    foreach (GameObject shield in blueShields) {
      Vector3 alignedPosition = targetPosition + forward * (index * alignDistance);
      shield.GetComponent<MovementController>().MoveTowards(alignedPosition, 5f);
      index++;
    }
    aligned = true;
  }
}
