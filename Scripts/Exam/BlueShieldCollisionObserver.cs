using UnityEngine;

public class Shield1CollisionObserver : MonoBehaviour {
  [SerializeField] private CollisionNotifier collisionNotifier;
  [SerializeField] private float movementSpeed = 2f;
  private MovementController movementController;
  [SerializeField] private string specialObjectTag = "Green Shield";

  private void Awake() {
    movementController = GetComponent<MovementController>();
  }

  private void OnEnable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected += OnCollisionDetected;
    }
  }

  private void OnDisable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected -= OnCollisionDetected;
    }
  }

  public void OnCollisionDetected(Collision other) {
    if (other.gameObject.CompareTag(specialObjectTag)) {
      GameObject footman = GameObject.FindWithTag("Footman");
      movementController.MoveTowards(footman.transform.position, movementSpeed);
    }
  }
}
