using UnityEngine;

/// <summary>
/// Exercise 8: Generic AI for enemies that seek and collect shields
/// Configurable to target any shield type
/// </summary>
public class HumanoidAI : MonoBehaviour {
  [Header("Target Shield")]
  [SerializeField] private string targetShieldTag = "Blue Shield";
  [SerializeField] private int pointsPerShield = 5;

  [Header("Movement Settings")]
  [SerializeField] private float moveSpeed = 5f;
  [SerializeField] private float detectionRadius = 50f;
  [SerializeField] private float collectRadius = 2f;

  private GameObject targetShield;

  private void Update() {
    if (targetShield == null || !targetShield.activeInHierarchy) {
      FindNearestShield();
    }
    if (targetShield != null) {
      MoveTowardsTarget();
      CheckShieldCollection();
    }
  }

  private void FindNearestShield() {
    GameObject[] shields = GameObject.FindGameObjectsWithTag(targetShieldTag);

    GameObject nearest = null;
    float minDistance = detectionRadius;

    foreach (GameObject shield in shields) {
      if (shield == null || !shield.activeInHierarchy)
        continue;
      float distance = Vector3.Distance(transform.position, shield.transform.position);
      if (distance < minDistance) {
        minDistance = distance;
        nearest = shield;
      }
    }
    targetShield = nearest;
  }

  private void MoveTowardsTarget() {
    if (targetShield == null)
      return;
    Vector3 direction = (targetShield.transform.position - transform.position).normalized;
    transform.position += direction * moveSpeed * Time.deltaTime;
    if (direction != Vector3.zero) {
      transform.rotation = Quaternion.LookRotation(direction);
    }
  }

  private void CheckShieldCollection() {
    if (targetShield == null)
      return;
    float distance = Vector3.Distance(transform.position, targetShield.transform.position);
    if (distance <= collectRadius) {
      CollectShield(targetShield);
    }
  }

  private void CollectShield(GameObject shield) {
    Debug.Log($"{gameObject.name} collected {targetShieldTag}! +{pointsPerShield} points");
    if (GameScoreManager.Instance != null) {
      GameScoreManager.Instance.AddEnemyScore(pointsPerShield);
    }
    shield.SetActive(false);
    targetShield = null;
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, detectionRadius);
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, collectRadius);
    if (targetShield != null) {
      Gizmos.color = Color.yellow;
      Gizmos.DrawLine(transform.position, targetShield.transform.position);
    }
  }
}
