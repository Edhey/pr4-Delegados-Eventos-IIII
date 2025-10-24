using UnityEngine;

/// <summary>
/// Simple Transform movement for an object
/// </summary>
public class TransformMove : MonoBehaviour {
  [SerializeField] private float speed = 10f;
  private Vector3 _cameraForward;
  private Vector3 _cameraRight;

  void Start() {
    GameObject cameraObject = GameObject.FindWithTag("MainCamera");
    if (cameraObject == null) {
      Debug.LogError("Main Camera not found at start.");
      return;
    }
    Transform cameraTransform = cameraObject.transform;
    _cameraForward = cameraTransform.forward;
    _cameraForward.y = 0;
    _cameraForward.Normalize();
    _cameraRight = cameraTransform.right;
    _cameraRight.y = 0;
    _cameraRight.Normalize();
  }

  void Update() {
    transform.Translate(GetMovementVector() * speed * Time.deltaTime, Space.World);
  }

  protected Vector3 GetMovementVector() {
    float horizontalAxis = Input.GetAxis("Horizontal");
    float verticalAxis = Input.GetAxis("Vertical");
    Vector3 movement = _cameraRight * horizontalAxis +
     _cameraForward * verticalAxis;
    if (movement.sqrMagnitude > 1f) {
      movement.Normalize();
    }
    return movement;
  }
}
