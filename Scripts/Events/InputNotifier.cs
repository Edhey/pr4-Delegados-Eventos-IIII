using UnityEngine;

/// <summary>
/// Notifies observers when a specific key is pressed
/// </summary>
public class InputNotifier : MonoBehaviour {
  [SerializeField] private KeyCode targetKey = KeyCode.Space;

  public delegate void KeyPressEventHandler();
  public event KeyPressEventHandler OnKeyPressed;

  private void Update() {
    if (Input.GetKeyDown(targetKey)) {
      OnKeyPressed?.Invoke();
    }
  }
}
