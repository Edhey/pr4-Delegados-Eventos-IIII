using UnityEngine;

public class OnClickMove : MonoBehaviour {
  [SerializeField] private Vector3 moveDirection = new Vector3(1, 0, 0);
  [SerializeField] private float speed = 2f;

  public void Move() {
    transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
  }
}