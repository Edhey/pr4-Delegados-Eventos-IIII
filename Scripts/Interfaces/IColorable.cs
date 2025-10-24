using UnityEngine;

/// <summary>
/// Interface for objects that can change color
/// </summary>
public interface IColorable {
  void ChangeColor(Color newColor);
  Color CurrentColor { get; }
}
