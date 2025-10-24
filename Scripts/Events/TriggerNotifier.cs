using UnityEngine;

/// <summary>
/// Notifies observers when a trigger is entered
/// </summary>
public class TriggerNotifier : MonoBehaviour {
  public delegate void TriggerEventHandler(Collider other);
  public event TriggerEventHandler OnTriggerDetected;

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Cube"))
      OnTriggerDetected?.Invoke(other);
  }
}
