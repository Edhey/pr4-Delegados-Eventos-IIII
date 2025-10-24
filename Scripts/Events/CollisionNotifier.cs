using UnityEngine;

/// <summary>
/// Notifies observers when a collision occurs
/// </summary>
public class CollisionNotifier : MonoBehaviour {
  public delegate void CollisionEventHandler(Collision collision);
  public event CollisionEventHandler OnCollisionDetected;

  private void OnCollisionEnter(Collision collision) {
    OnCollisionDetected?.Invoke(collision);
  }
}
