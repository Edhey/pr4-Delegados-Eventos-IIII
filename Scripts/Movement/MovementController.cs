using UnityEngine;

/// <summary>
/// Base component for moving objects smoothly
/// Implements IMovable interface
/// </summary>
public class MovementController : MonoBehaviour, IMovable {
  [SerializeField] private float defaultSpeed = 5f;
  [SerializeField] private float stopDistance = 0.1f;

  private Vector3 targetPosition;
  private float currentSpeed;
  private bool shouldMove = false;

  public bool IsMoving { get; private set; }

  public void MoveTo(Vector3 targetPosition) {
    transform.position = targetPosition;
    shouldMove = false;
    IsMoving = false;
  }

  public void MoveTowards(Vector3 targetPosition, float speed) {
    this.targetPosition = targetPosition;
    this.currentSpeed = speed;
    shouldMove = true;
    IsMoving = true;
  }

  public void MoveTowardsDefault(Vector3 targetPosition) {
    MoveTowards(targetPosition, defaultSpeed);
  }

  private void Update() {
    if (shouldMove) {
      transform.position = Vector3.MoveTowards(
        transform.position,
        targetPosition,
        currentSpeed * Time.deltaTime
      );

      if (Vector3.Distance(transform.position, targetPosition) <= stopDistance) {
        transform.position = targetPosition;
        shouldMove = false;
        IsMoving = false;
      }
    }
  }

  public void StopMovement() {
    shouldMove = false;
    IsMoving = false;
  }
}
