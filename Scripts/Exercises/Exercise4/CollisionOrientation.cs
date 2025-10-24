using UnityEngine;

/// <summary>
/// Exercise 4: Orients object to look at target on collision
/// </summary>
public class CollisionOrientation : MonoBehaviour {
  [SerializeField] private CollisionNotifier cubeNotifier;
  [SerializeField] private Transform targetToLookAt;
  [SerializeField] private GameObject referenceObject;

  private void OnEnable() {
    if (cubeNotifier != null) {
      cubeNotifier.OnCollisionDetected += HandleCollision;
    }
  }

  private void OnDisable() {
    if (cubeNotifier != null) {
      cubeNotifier.OnCollisionDetected -= HandleCollision;
    }
  }

  private void HandleCollision(Collision collision) {
    if (collision.gameObject.CompareTag(referenceObject.tag)) {
      if (targetToLookAt != null) {
        transform.LookAt(targetToLookAt);
      }
    }
  }
}
