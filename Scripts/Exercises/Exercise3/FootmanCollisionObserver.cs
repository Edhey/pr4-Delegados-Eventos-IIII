using UnityEngine;

/// <summary>
/// Controller for the Footman when cube collision is detected.
/// </summary>
public class FootmanCollisionMover : MonoBehaviour, ICollisionObserver {
  [SerializeField] private CollisionNotifier collisionNotifier;
  [SerializeField] private GameObject shield;
  [SerializeField] private float moveSpeed = 6f;
  [SerializeField] private GameObject referenceObject;

  private MovementController movement;
  private ColorController colorController;
  private string shieldTagGroup2 = "Purple Shield";

  private void Awake() {
    movement = GetComponent<MovementController>();
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
    if (other.gameObject.CompareTag("Lich")) {
      if (shield != null) {
        movement.MoveTowards(shield.transform.position, moveSpeed);
        Debug.Log($"{gameObject.name} moving to selected shield");
      }
    }
    else if (other.gameObject.CompareTag("Footman")) {
      MoveToClosestShield(shieldTagGroup2);
    } else if (other.gameObject.CompareTag(referenceObject.tag)) {
      transform.position = referenceObject.transform.position;
    }
  }

  private void MoveToClosestShield(string shieldTag) {
    GameObject[] shields = GameObject.FindGameObjectsWithTag(shieldTag);

    if (shields.Length == 0) {
      Debug.LogWarning($"No {shieldTag} found!");
      return;
    }

    GameObject closest = GetClosestShield(shields);
    if (closest != null) {
      movement.MoveTowards(closest.transform.position, moveSpeed);
      Debug.Log($"{gameObject.name} moving to closest shield: {closest.name}");
    }
  }

  private GameObject GetClosestShield(GameObject[] shields) {
    GameObject closest = null;
    float minDistance = Mathf.Infinity;

    foreach (GameObject shield in shields) {
      if (shield == null)
        continue;

      float distance = Vector3.Distance(
        transform.position,
        shield.transform.position
      );

      if (distance < minDistance) {
        minDistance = distance;
        closest = shield;
      }
    }

    return closest;
  }

  void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.CompareTag(shieldTagGroup2)) {
      if (colorController != null) {
        colorController.ChangeColorRandom();
        Debug.Log($"{gameObject.name} collided with {shieldTagGroup2} - color changed!");
      }
      else {
        Debug.LogError($"{gameObject.name} has no ColorController component!");
      }
    }
  }
}
