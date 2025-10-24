using UnityEngine;

/// <summary>
/// Interface for objects that need to be notified of trigger events
/// </summary>
public interface ITriggerObserver {
  void OnTriggerDetected(Collider other);
}
