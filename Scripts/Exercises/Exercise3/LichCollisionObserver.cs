using UnityEngine;

/// <summary>
/// Controller for Lich (Type 2 Humanoid)
/// When cube collides with any Lich, all Footmen move to selected shield
/// </summary>
public class LichCollisionObserver : MonoBehaviour, ICollisionObserver {
  [SerializeField] private CollisionNotifier cubeCollisionNotifier;
  [SerializeField] private GameObject selectedShield;
  [SerializeField] private float moveSpeed = 6f;

  private void OnEnable() {
    if (cubeCollisionNotifier != null) {
      cubeCollisionNotifier.OnCollisionDetected += OnCollisionDetected;
    }
  }

  private void OnDisable() {
    if (cubeCollisionNotifier != null) {
      cubeCollisionNotifier.OnCollisionDetected -= OnCollisionDetected;
    }
  }

  public void OnCollisionDetected(Collision other) {
    // Only react if cube hit a Lich (Type 2)
    if (other.gameObject.CompareTag("Lich")) {
      MoveAllFootmenToSelectedShield();
    }
  }

  private void MoveAllFootmenToSelectedShield() {
    if (selectedShield == null) {
      Debug.LogWarning("No selected shield assigned!");
      return;
    }

    GameObject[] footmen = GameObject.FindGameObjectsWithTag("Footman");

    if (footmen.Length == 0) {
      Debug.LogWarning("No Footmen found in scene!");
      return;
    }

    Debug.Log($"Lich triggered: Moving all Footmen to {selectedShield.name}");

    foreach (GameObject footman in footmen) {
      MovementController movement =
        footman.GetComponent<MovementController>();

      if (movement != null) {
        movement.MoveTowards(
          selectedShield.transform.position,
          moveSpeed
        );
        Debug.Log($"  - {footman.name} moving to selected shield");
      }
    }
  }
}
