using UnityEngine;

public class CollisionNotificator : MonoBehaviour {
  public delegate void CollisionAction();
  public event CollisionAction OnCollision;

  private void OnCollisionEnter(Collision collision) {
    OnCollision?.Invoke();
  }
}
