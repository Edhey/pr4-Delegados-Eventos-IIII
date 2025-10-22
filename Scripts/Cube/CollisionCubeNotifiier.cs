using UnityEngine;

public class CollisionCubeNotifier : MonoBehaviour {
  public delegate void FootmanCollision();
  public static event FootmanCollision OnFootmanCollision;
  public delegate void LichCollision();
  public static event LichCollision OnLichCollision;
  
  private void OnCollisionEnter(Collision collision) {
    switch (gameObject.tag) {
      case "Footman":
        OnFootmanCollision?.Invoke();
        break;
      case "Lich":
        OnLichCollision?.Invoke();
        break;
    }
  }
}
