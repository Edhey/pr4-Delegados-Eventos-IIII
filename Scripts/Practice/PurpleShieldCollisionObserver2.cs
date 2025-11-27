using UnityEngine;

public class PurpleShieldCollisionObserver2 : MonoBehaviour {
  [SerializeField] private CollisionNotifier collisionNotifier;
  [SerializeField] private float scaleIncreaseFactor = 1.5f;
  private ColorController colorController;

  private void Awake() {
    colorController = GetComponent<ColorController>();
  }

  private void OnEnable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected += OnCollisionDetected;
    }
  }

  private void OnDisable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected -= OnCollisionDetected;
    }
  }

  public void OnCollisionDetected(Collision other) {
    if (other.gameObject.CompareTag("Footman")) {
      colorController.ChangeColorRandom();
    }
    else if (other.gameObject.CompareTag("Lich")) {
      transform.localScale *= scaleIncreaseFactor;
    }
  }
}
