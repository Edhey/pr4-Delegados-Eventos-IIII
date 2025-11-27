using UnityEngine;

public class ScatterFromObject : MonoBehaviour {
  [SerializeField] private float scatterDistance = 5f;
  [SerializeField] private GameObject scatterFrom;
  [SerializeField] private CollisionNotifier collisionNotifier;
  private MovementController movementController;

  private void Awake() {
    if (movementController == null) {
      movementController = GetComponent<MovementController>();
    }
  }

  private void OnEnable() {
    collisionNotifier.OnCollisionDetected += OnCollisionDetected;
  }
  private void OnDisable() {
    collisionNotifier.OnCollisionDetected -= OnCollisionDetected;
  }

  private void OnCollisionDetected(Collision collision) {
    if (collision.gameObject == scatterFrom) {
      movementController.ScatterFrom(scatterFrom.transform.position, scatterDistance);
    }
  }
}
