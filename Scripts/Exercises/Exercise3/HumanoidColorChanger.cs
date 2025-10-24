using UnityEngine;

/// <summary>
/// Exercise 3: Changes humanoid color when sphere collides
/// </summary>
public class HumanoidColorChanger : MonoBehaviour {
  [SerializeField] private CollisionNotifier sphereNotifier;
  [SerializeField] private Color targetColor = Color.blue;

  private ColorController colorController;

  private void Awake() {
    colorController = GetComponent<ColorController>();
  }

  private void OnEnable() {
    if (sphereNotifier != null) {
      sphereNotifier.OnCollisionDetected += HandleCollision;
    }
  }

  private void OnDisable() {
    if (sphereNotifier != null) {
      sphereNotifier.OnCollisionDetected -= HandleCollision;
    }
  }

  private void HandleCollision(Collision collision) {
    if (collision.gameObject.CompareTag("Cylinder")) {
      colorController.ChangeColor(targetColor);
    }
  }
}
