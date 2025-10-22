using UnityEngine;
using UnityEngine.UIElements;

public class MoveToObjectObserver : MonoBehaviour {
  [SerializeField] private CollisionNotificator collisionNotificator;
  [SerializeField] private Transform targetObject;

  void Start() {
    collisionNotificator.OnCollision += MoveToObject;
  }

  void MoveToObject() {
    transform.position = targetObject.position;
  }
}
