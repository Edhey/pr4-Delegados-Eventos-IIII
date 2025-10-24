using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Exercise 8: Prototype scene manager
/// </summary>
public class SceneManager : MonoBehaviour {
  [SerializeField] private int winScore = 500;
  [Header("UI")]
  [SerializeField] private Image winImage;
  [SerializeField] private Image loseImage;
  [SerializeField] private Button restartButton;

  [Header("Game Objects")]
  [SerializeField] private GameObject playerObject;
  [SerializeField] private GameObject[] enemyObjects;
  private Vector3 playerInitialPosition;
  private Quaternion playerInitialRotation;
  private Vector3[] enemyInitialPositions;
  private Quaternion[] enemyInitialRotations;

  private void Start() {
    if (playerObject != null) {
      playerInitialPosition = playerObject.transform.position;
      playerInitialRotation = playerObject.transform.rotation;
    }

    if (enemyObjects != null && enemyObjects.Length > 0) {
      enemyInitialPositions = new Vector3[enemyObjects.Length];
      enemyInitialRotations = new Quaternion[enemyObjects.Length];

      for (int i = 0; i < enemyObjects.Length; i++) {
        if (enemyObjects[i] != null) {
          enemyInitialPositions[i] = enemyObjects[i].transform.position;
          enemyInitialRotations[i] = enemyObjects[i].transform.rotation;
        }
      }
    }
  }

  private void OnEnable() {
    if (GameScoreManager.Instance != null) {
      GameScoreManager.Instance.OnPlayerScoreChanged += CheckWinCondition;
      GameScoreManager.Instance.OnEnemyScoreChanged += CheckLoseCondition;
    }
    else {
      Debug.LogError("GameScoreManager instance not found.");
    }
    if (winImage != null)
      winImage.gameObject.SetActive(false);
    else
      Debug.LogWarning("Win Image reference is missing.");
    if (loseImage != null)
      loseImage.gameObject.SetActive(false);
    else
      Debug.LogWarning("Lose Image reference is missing.");
    if (restartButton != null)
      restartButton.gameObject.SetActive(false);
    else
      Debug.LogError("Restart Button reference is missing.");
  }

  private void OnDisable() {
    if (GameScoreManager.Instance != null) {
      GameScoreManager.Instance.OnPlayerScoreChanged -= CheckWinCondition;
      GameScoreManager.Instance.OnEnemyScoreChanged -= CheckLoseCondition;
    }
  }

  private void CheckWinCondition(int currentScore) {
    if (currentScore >= winScore) {
      OnGameWon();
    }
  }

  private void CheckLoseCondition(int enemyScore) {
    if (enemyScore >= winScore) {
      OnGameLost();
    }
  }

  private void OnGameWon() {
    Debug.Log("You won!");
    if (winImage != null)
      winImage.gameObject.SetActive(true);
    if (restartButton != null)
      restartButton.gameObject.SetActive(true);
    Time.timeScale = 0f;
  }

  private void OnGameLost() {
    Debug.Log("You lost!");
    if (loseImage != null)
      loseImage.gameObject.SetActive(true);
    if (restartButton != null)
      restartButton.gameObject.SetActive(true);
    Time.timeScale = 0f;
  }

  public void ResetScene() {
    if (GameScoreManager.Instance != null) {
      GameScoreManager.Instance.ResetScore();
    }

    DestroyAllShields();

    if (playerObject != null) {
      playerObject.transform.position = playerInitialPosition;
      playerObject.transform.rotation = playerInitialRotation;
      Rigidbody playerRb = playerObject.GetComponent<Rigidbody>();
      if (playerRb != null) {
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
      }
    }

    if (enemyObjects != null && enemyInitialPositions != null) {
      for (int i = 0; i < enemyObjects.Length; i++) {
        if (enemyObjects[i] != null && i < enemyInitialPositions.Length) {
          enemyObjects[i].transform.position = enemyInitialPositions[i];
          enemyObjects[i].transform.rotation = enemyInitialRotations[i];
          Rigidbody enemyRb = enemyObjects[i].GetComponent<Rigidbody>();
          if (enemyRb != null) {
            enemyRb.linearVelocity = Vector3.zero;
            enemyRb.angularVelocity = Vector3.zero;
          }
        }
      }
    }

    if (winImage != null)
      winImage.gameObject.SetActive(false);
    if (loseImage != null)
      loseImage.gameObject.SetActive(false);
    if (restartButton != null)
      restartButton.gameObject.SetActive(false);

    Time.timeScale = 1f;
    Debug.Log("Scene reset - all objects returned to initial positions.");
  }

  public void OnRestartButtonPressed() {
    ResetScene();
    Debug.Log("Restart button pressed - scene reset.");
  }

  private void DestroyAllShields() {
    GameObject[] blueShields = GameObject.FindGameObjectsWithTag("Blue Shield");
    GameObject[] purpleShields = GameObject.FindGameObjectsWithTag("Purple Shield");

    foreach (GameObject shield in blueShields) {
      if (shield != null) {
        Destroy(shield);
      }
    }

    foreach (GameObject shield in purpleShields) {
      if (shield != null) {
        Destroy(shield);
      }
    }
  }
}
