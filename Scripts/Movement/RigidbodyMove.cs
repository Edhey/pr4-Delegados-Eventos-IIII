using UnityEngine;

/// <summary>
/// Simple Rigidbody-based movement controller
/// </summary>
public class RigidBodyMove : MonoBehaviour {
  [SerializeField] private float speed = 10f;
  private Rigidbody rb;

  void Start() {
    rb = GetComponent<Rigidbody>();
  }

  void Update() {
    float verticalInput = Input.GetAxis("Vertical");
    float horizontalInput = Input.GetAxis("Horizontal");

    Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;
    rb.AddForce(direction);
  }
}
