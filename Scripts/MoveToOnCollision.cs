using UnityEngine;

public class MoveToOnCollision : MonoBehaviour, ICollisionObserver {
  [SerializeField] private CollisionNotificator collisionNotificator;
  [SerializeField] private Transform targetObject;

  private void Start() {
    collisionNotificator.OnCollision += OnNotified;
  }

  public void OnNotified() {
    MoveToTarget();
  }

  private void MoveToTarget() {
    transform.position = targetObject.position;
  }
}
