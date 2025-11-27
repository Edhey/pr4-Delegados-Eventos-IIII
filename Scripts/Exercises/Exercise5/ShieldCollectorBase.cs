using UnityEngine;

/// <summary>
/// Exercise 5: Collects shields and updates score
/// </summary>
public class ShieldCollectorBase : MonoBehaviour {
  [SerializeField] private CollisionNotifier collisionNotifier;
  public int pointsPerType1Shield = 5;
  public int pointsPerType2Shield = 10;
  [SerializeField] private string type1ShieldTag = "Blue Shield";
  [SerializeField] private string type2ShieldTag = "Purple Shield";

  private void OnEnable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected += HandleCollision;
    }
  }

  private void OnDisable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected -= HandleCollision;
    }
  }

  private void HandleCollision(Collision collision) {
    if (collision.gameObject.CompareTag(type1ShieldTag) || collision.gameObject.CompareTag(type2ShieldTag)) {
      CollectShield(collision.gameObject);
    }
  }

  private void CollectShield(GameObject shield) {
    if (ScoreManager.Instance != null) {
      if (shield.CompareTag(type1ShieldTag))
        ScoreManager.Instance.AddScore(pointsPerType1Shield);
      else if (shield.CompareTag(type2ShieldTag))
        ScoreManager.Instance.AddScore(pointsPerType2Shield);
    }
    shield.SetActive(false);
  }

  public void changePointsPerType1Shield(int newPoints) {
    pointsPerType1Shield = newPoints;
  }

  public void changePointsPerType2Shield(int newPoints) {
    pointsPerType2Shield = newPoints;
  }
}
