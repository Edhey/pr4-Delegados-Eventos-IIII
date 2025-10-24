using UnityEngine;

/// <summary>
/// Exercise 4: Teleports object to spawn point on collision
/// </summary>
public class CollisionTeleporter : MonoBehaviour {
  [SerializeField] private CollisionNotifier collisionNotifier;
  [SerializeField] private GameObject referenceObject;

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
    if (referenceObject != null && collision.gameObject.CompareTag(referenceObject.tag)) {
      transform.position = referenceObject.transform.position;
    }
  }
}
