using UnityEngine;

public class OnCollisionChangeColor : MonoBehaviour {
  [SerializeField] private CollisionNotificator collisionNotificator;

  void Start() {
    collisionNotificator.OnCollision += ChangeColor;
  }

  void ChangeColor() {
    GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
  }
}
