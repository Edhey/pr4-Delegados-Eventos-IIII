using UnityEngine;

/// <summary>
/// Exercise 2: Manages humanoid movement on collision with the 
/// </summary>
public class HumanoidTriggerMover : MonoBehaviour, ITriggerObserver {
  [SerializeField] private TriggerNotifier triggerNotifier;
  [SerializeField] private Transform targetPosition;
  [SerializeField] private float moveSpeed = 3f;

  private MovementController movement;

  private void Awake() {
    movement = GetComponent<MovementController>();
  }

  private void OnEnable() {
    if (triggerNotifier != null) {
      triggerNotifier.OnTriggerDetected += OnTriggerDetected;
    }
  }

  private void OnDisable() {
    if (triggerNotifier != null) {
      triggerNotifier.OnTriggerDetected -= OnTriggerDetected;
    }
  }

  public void OnTriggerDetected(Collider other) {
    if (targetPosition != null) {
      movement.MoveTowards(targetPosition.position, moveSpeed);
    }
  }
}
