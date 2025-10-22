using UnityEngine;

public class SmoothMoveToOnCollision : MonoBehaviour, ICollisionObserver {
  [SerializeField] private CollisionNotificator collisionNotificator;
  [SerializeField] private Transform targetObject;
  [SerializeField] private float speed = 5f;
  private bool shouldMove = false;

  private void Start() {
    collisionNotificator.OnCollision += OnNotified;
  }

  public void OnNotified() {
    shouldMove = true;
  }

  private void Update() {
    if (shouldMove) {
      transform.position = Vector3.MoveTowards(transform.position, targetObject.position, speed * Time.deltaTime);
      if (transform.position == targetObject.position) {
        shouldMove = false;
      }
    }
  }
}