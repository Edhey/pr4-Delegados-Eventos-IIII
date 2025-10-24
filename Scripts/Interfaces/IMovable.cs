using UnityEngine;

/// <summary>
/// Interface for objects that can move
/// </summary>
public interface IMovable {
  void MoveTo(Vector3 targetPosition);
  void MoveTowards(Vector3 targetPosition, float speed);
  bool IsMoving { get; }
}
