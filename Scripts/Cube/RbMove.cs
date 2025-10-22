using UnityEngine;

public class RbMove : MonoBehaviour {
  [SerializeField] private float speed = 10f;
  private Rigidbody rb;

  void Start() {
    rb = GetComponent<Rigidbody>();
  }

  void Update() {
    float verticalInput = Input.GetAxis("Vertical");
    float horizontalInput = Input.GetAxis("Horizontal");

    Vector3 direction = transform.forward * verticalInput + transform.right * horizontalInput;
    Vector3 movement = direction.normalized * speed;
    Vector3 newPosition = rb.position + movement * Time.deltaTime;

    rb.MovePosition(newPosition);
  }
}
