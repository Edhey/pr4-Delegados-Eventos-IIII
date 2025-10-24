using UnityEngine;

/// <summary>
/// Base component for changing object colors
/// Implements IColorable interface
/// </summary>
public class ColorController : MonoBehaviour, IColorable {
  private Renderer[] objectRenderers;
  private MaterialPropertyBlock propertyBlock;
  public Color CurrentColor { get; private set; }

  private void Awake() {
    objectRenderers = GetComponentsInChildren<Renderer>();
    if (objectRenderers == null || objectRenderers.Length == 0) {
      Debug.LogError($"No Renderer found on {gameObject.name} or its children.");
      return;
    }
    propertyBlock = new MaterialPropertyBlock();
    if (objectRenderers[0].material != null) {
      CurrentColor = objectRenderers[0].material.color;
    }
    // Debug.Log($"ColorController on {gameObject.name}: Found {objectRenderers.Length} renderer(s)");
  }

  public void ChangeColor(Color newColor) {
    if (objectRenderers == null || objectRenderers.Length == 0) {
      Debug.LogWarning($"Cannot change color on {gameObject.name}: No renderers available");
      return;
    }
    CurrentColor = newColor;
    foreach (Renderer renderer in objectRenderers) {
      if (renderer == null)
        continue;
      renderer.GetPropertyBlock(propertyBlock);
      propertyBlock.SetColor("_Color", newColor);
      propertyBlock.SetColor("_BaseColor", newColor);
      propertyBlock.SetColor("_MainColor", newColor);
      renderer.SetPropertyBlock(propertyBlock);
    }
    // Debug.Log($"{gameObject.name} color changed to {newColor}");
  }

  public void ChangeColorRandom() {
    Color randomColor = new Color(
      Random.Range(0f, 1f),
      Random.Range(0f, 1f),
      Random.Range(0f, 1f)
    );
    ChangeColor(randomColor);
  }
}
