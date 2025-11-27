using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PurpleShieldCollisionObserver : MonoBehaviour {
  [SerializeField] private CollisionNotifier collisionNotifier;
  private MovementController movementController;
  [SerializeField] private string specialObjectTag = "Green Shield";
  [SerializeField] private int scoreThreshold = 100;
  [SerializeField] private float scaleMultiplier = 1.5f;
  private bool scaled = false;

  private void Awake() {
    movementController = GetComponent<MovementController>();
  }

  private void OnEnable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected += OnCollisionDetected;
    }
    if (ScoreManager.Instance != null) {
      ScoreManager.Instance.OnScoreChanged += CheckScore;
    }
  }

  private void OnDisable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected -= OnCollisionDetected;
    }
    if (ScoreManager.Instance != null) {
      ScoreManager.Instance.OnScoreChanged -= CheckScore;
    }
  }

  public void OnCollisionDetected(Collision other) {
    if (other.gameObject.CompareTag(specialObjectTag)) {
      GameObject footman = GameObject.FindWithTag("Footman");
      movementController.ScatterFrom(footman.transform.position, 5f);
    }
  }

  private void CheckScore(int currentScore) {
    if (currentScore >= scoreThreshold && !scaled) {
      transform.localScale *= scaleMultiplier;
      scaled = true;
    }
  }
}
