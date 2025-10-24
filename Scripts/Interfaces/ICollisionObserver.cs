using UnityEngine;

/// <summary>
/// Interface for objects that need to be notified of collisions
/// </summary>
public interface ICollisionObserver {
  void OnCollisionDetected(Collision collision);
}
  