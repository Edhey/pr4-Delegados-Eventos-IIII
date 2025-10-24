using UnityEngine;

/// <summary>
/// Exercise 1: Moves sphere to a target when cube collides
/// </summary>
public class SphereCollisionMover : MonoBehaviour {
  [SerializeField] private CollisionNotifier cubeNotifier;
  [SerializeField] private Transform targetPosition;
  [SerializeField] private float moveSpeed = 5f;

  private MovementController movement;

  private void Awake() {
    movement = GetComponent<MovementController>();
  }

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
    if (targetPosition != null) {
      movement.MoveTowards(targetPosition.position, moveSpeed);
    }
  }
}
