using UnityEngine;

/// <summary>
/// Exercise 5: Collects shields using trigger and updates score
/// </summary>
public class TriggerShieldCollector : MonoBehaviour
{
  [SerializeField] private TriggerNotifier triggerNotifier;
  [SerializeField] private int pointsPerType1Shield = 5;
  [SerializeField] private int pointsPerType2Shield = 10;
  [SerializeField] private string type1ShieldTag = "Blue Shield";
  [SerializeField] private string type2ShieldTag = "Purple Shield";

  private void OnEnable()
  {
    if (triggerNotifier != null)
    {
      triggerNotifier.OnTriggerDetected += HandleTrigger;
    }
  }

  private void OnDisable()
  {
    if (triggerNotifier != null)
    {
      triggerNotifier.OnTriggerDetected -= HandleTrigger;
    }
  }

  private void HandleTrigger(Collider other)
  {
    if (other.CompareTag(type1ShieldTag) || other.CompareTag(type2ShieldTag))
    {
      CollectShield(other.gameObject);
    }
  }

  private void CollectShield(GameObject shield)
  {
    if (ScoreManager.Instance != null)
    {
      if (shield.CompareTag(type1ShieldTag))
        ScoreManager.Instance.AddScore(pointsPerType1Shield);
      else if (shield.CompareTag(type2ShieldTag))
        ScoreManager.Instance.AddScore(pointsPerType2Shield);
    }
    shield.SetActive(false);
  }
}