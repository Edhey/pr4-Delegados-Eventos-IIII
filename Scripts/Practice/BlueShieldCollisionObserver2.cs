using UnityEngine;

public class Shield1CollisionObserver2 : MonoBehaviour {
  [SerializeField] private CollisionNotifier collisionNotifier;
  [SerializeField] private Vector3 gatheringPoint = new Vector3(0, 0, 0);
  [SerializeField] private float scatterDistance = 5f;
  [SerializeField] private float movementSpeed = 2f;
  private MovementController movementController;

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
    if (other.gameObject.CompareTag("Footman")) {
      movementController.MoveTowards(gatheringPoint, movementSpeed);
    }
    else if (other.gameObject.CompareTag("Lich")) {
      GameObject footman = GameObject.FindWithTag("Footman");
      Vector3 dir = footman != null ? (transform.position - footman.transform.position).normalized : Random.onUnitSphere;
      if (dir == Vector3.zero)
        dir = Random.onUnitSphere;
      transform.position += dir * scatterDistance;
    }
  }
}
