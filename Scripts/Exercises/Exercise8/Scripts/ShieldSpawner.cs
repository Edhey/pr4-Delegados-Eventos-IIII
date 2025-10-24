using UnityEngine;

/// <summary>
/// Exercise 8: Spawns shields randomly in the scene
/// </summary>
public class ShieldSpawner : MonoBehaviour {
  [Header("Shield Prefabs")]
  [SerializeField] private GameObject type1ShieldPrefab;
  [SerializeField] private GameObject type2ShieldPrefab;

  [Header("Spawn Settings")]
  [SerializeField] private float spawnInterval = 5f;
  [SerializeField] private int maxShieldsInScene = 5;
  [SerializeField] private Vector3 spawnAreaMin = new Vector3(-50, 0, -50);
  [SerializeField] private Vector3 spawnAreaMax = new Vector3(50, 0, 50);
  [SerializeField] private float spawnHeight = 1f;

  [Header("Spawn Probability")]
  [Range(0f, 1f)]
  [SerializeField] private float type1Probability = 0.6f; // 60% type 1, 40% type 2

  private float spawnTimer;
  private int currentShieldCount;

  private void Update() {
    spawnTimer += Time.deltaTime;
    if (spawnTimer >= spawnInterval) {
      spawnTimer = 0f;
      TrySpawnShield();
    }
  }

  private void TrySpawnShield() {
    GameObject[] type1Shields = GameObject.FindGameObjectsWithTag("Blue Shield");
    GameObject[] type2Shields = GameObject.FindGameObjectsWithTag("Purple Shield");
    currentShieldCount = type1Shields.Length + type2Shields.Length;
    if (currentShieldCount >= maxShieldsInScene)
      return;
    SpawnRandomShield();
  }

  private void SpawnRandomShield() {
    GameObject shieldPrefab = Random.value <= type1Probability ? type1ShieldPrefab : type2ShieldPrefab;
    if (shieldPrefab == null) {
      Debug.LogWarning("Shield prefab not assigned!");
      return;
    }
    Vector3 randomPosition = new Vector3(
      Random.Range(spawnAreaMin.x, spawnAreaMax.x),
      spawnHeight,
      Random.Range(spawnAreaMin.z, spawnAreaMax.z)
    );
    GameObject newShield = Instantiate(shieldPrefab, randomPosition, Quaternion.identity);
    Debug.Log($"Shield spawned at {randomPosition}");
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.yellow;
    Vector3 center = (spawnAreaMin + spawnAreaMax) / 2f;
    Vector3 size = spawnAreaMax - spawnAreaMin;
    Gizmos.DrawWireCube(center, size);
  }
}
