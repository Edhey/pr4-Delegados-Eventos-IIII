# Práctica 4 Interfaces Inteligentes. Delegados y Eventos

- **Author**: Himar Edhey Hernández Alonso
- **Subject**: Interfaces Inteligentes
- **Course**: Interfaces Inteligentes

## Introducction

In this practice, we will implement a scene in Unity where we will use delegates and events to create interactions between different game objects. The main objective is to understand how to use these programming concepts to manage communication between objects in a decoupled way.

## Ejercicio Extra (Examen Parecido)

Crear una escena donde al pulsar un botón se notifique a una esfera y a un cubo que deben desplazarse, además al chocar con un cilindro todos deben cambiar de color.

I created a scene with a cube, a sphere, a cylinder and a plane. I added a UI Canvas with a button to the scene. The button has an OnClick event that notifies the cube and sphere to move when clicked. Then I used the following scripts to manage the interactions:

```csharp
using UnityEngine;

public class CollisionNotificator : MonoBehaviour {
  public delegate void CollisionAction();
  public event CollisionAction OnCollision;

  private void OnCollisionEnter(Collision collision) {
    OnCollision?.Invoke();
  }
}
```

This script detects collisions and notifies subscribed objects when a collision occurs. It was added to the cylinder.

```csharp
using UnityEngine;

public class OnClickMove : MonoBehaviour {
  [SerializeField] private Vector3 moveDirection = new Vector3(1, 0, 0);
  [SerializeField] private float speed = 2f;

  public void Move() {
    transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
  }
}
```

This one is used to move the cube and sphere when the button is clicked. The Move method is linked to the button's OnClick event and attached to both the cube and the sphere.

```csharp
using UnityEngine;

public class OnCollisionChangeColor : MonoBehaviour {
  [SerializeField] private CollisionNotificator collisionNotificator;

  void Start() {
    collisionNotificator.OnCollision += ChangeColor;s
  }

  void ChangeColor() {
    GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
  }
}
```

The last one is used to change the color of the object attached to when a collision is detected with the CollisionNotificator. In this case I used the cylinder's CollisionNotificator to notify all to change their colors when they collide with the cylinder.

![Extra Exercise Interaction](Resources/Pr4-Extra-Exercise.gif)

## Events and Delegates Exercises

### Exercise 1

*Crea una escena con 5 esferas, rojas que las etiquetas de tipo 1, y verdes de tipo 2. Añade un cubo y un cilindro. Crea la siguiente mecánica: cuando el cubo colisiona con el cilindro, las esferas de tipo 1 se dirigen hacia una de las esferas de tipo 2 que fijes de antemano y las esferas de tipo 2 se desplazan hacia el cilindro.*

For this exercise, ten spheres were placed on the terrain used in previous practices, slightly modifying the area to make room. Five spheres were assigned a blue material and the other five a green material, and each one was tagged with its corresponding type. A cube and a cylinder were also added to the scene. The cylinder was given a red material, and the cube a gold one.

I added to the cube a Rigidbody component to make it a physical object, allowing it to collide with the cylinder. The cylinder was given a Box Collider component to detect collisions. So, for moving the cube, I created a simple script that allows movement using the arrow keys:

```csharp
using UnityEngine;

/// <summary>
/// Simple Rigidbody-based movement controller
/// </summary>
public class RigidBodyMove : MonoBehaviour {
  [SerializeField] private float speed = 10f;
  private Rigidbody rb;

  void Start() {
    rb = GetComponent<Rigidbody>();
  }

  void Update() {
    float verticalInput = Input.GetAxis("Vertical");
    float horizontalInput = Input.GetAxis("Horizontal");

    Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;
    rb.AddForce(direction);
  }
}

```

For managing the interactions, I created a script called  CollisionNotificator, this script is responsible for detecting when the object attached to it collides with another object, sending notifications to subscribed objects when it occurs.

```csharp
using UnityEngine;

public class CollisionNotificator : MonoBehaviour {
  public delegate void CollisionAction();
  public event CollisionAction OnCollision;

  private void OnCollisionEnter(Collision collision) {
    OnCollision?.Invoke();
  }
}
```

Next, I created a Interface called ICollisionObserver, which defines a method that objects must implement to respond to collision notifications.

```csharp
public interface ICollisionObserver {
  void OnNotified();
}
```

With this I can use the observer pattern to manage the interactions between the objects in a clean way. For that I created another script which implements the ICollisionObserver interface. First, for moving the objects I implemented the movement as a teleportation to the target position:

```csharp
using UnityEngine;

public class MoveToOnCollision : MonoBehaviour, ICollisionObserver {
  [SerializeField] private CollisionNotificator collisionNotificator;
  [SerializeField] private Transform targetObject;

  private void Start() {
    collisionNotificator.OnCollision += OnNotified;
  }

  public void OnNotified() {
    MoveToTarget();
  }

  private void MoveToTarget() {
    transform.position = targetObject.position;
  }
}
```

![On Collision Teleportation](Resources/Pr4-Ej1-teleportation.gif)

Then, I implemented another script where I move the objects in a smooth way towards the target position:

```csharp
using UnityEngine;

public class SmoothMoveToOnCollision : MonoBehaviour, ICollisionObserver {
  [SerializeField] private CollisionNotificator collisionNotificator;
  [SerializeField] private Transform targetObject;
  [SerializeField] private float speed = 5f;
  private bool shouldMove = false;

  private void Start() {
    collisionNotificator.OnCollision += OnNotified;
  }

  public void OnNotified() {
    shouldMove = true;
  }

  private void Update() {
    if (shouldMove) {
      transform.position = Vector3.MoveTowards(transform.position, targetObject.position, speed * Time.deltaTime);
      if (transform.position == targetObject.position) {
        shouldMove = false;
      }
    }
  }
}
```

![On Collision Smooth Movement](Resources/Pr4-Ej1-smooth-movement.gif)

### Exercise 2

*Sustituye los objetos geométricos por humanoides.*

For this I replaced the spheres with humanoid models, assigning them the tags corresponding to their types.

![Humanoids Scene](Resources/humanoids-scene.png)

I changed the scripts used before to use `TriggerNotifier` instead of `CollisionNotifier`, for practice more, so I made `IsTrigger` true in the cylinder collider. The rest of the logic is quite the same as before. I also added a `MovementController` component to the humanoids to handle their movement smoothly and delegated the movement logic to that component, that implements the interface `IMovable`. This way I implemented another type of movement where the humanoids move towards the fixed target position when the collision occurs, instead of the chasing movement used before.

For making the move without rigidbody physics, I created the `TransformMove` script that implements the `IMovable` interface:

```csharp
using UnityEngine;

/// <summary>
/// Simple Transform movement for an object
/// </summary>
public class TransformMove : MonoBehaviour {
  [SerializeField] private float speed = 10f;
  private Vector3 _cameraForward;
  private Vector3 _cameraRight;

  void Start() {
    GameObject cameraObject = GameObject.FindWithTag("MainCamera");
    if (cameraObject == null) {
      Debug.LogError("Main Camera not found at start.");
      return;
    }
    Transform cameraTransform = cameraObject.transform;
    _cameraForward = cameraTransform.forward;
    _cameraForward.y = 0;
    _cameraForward.Normalize();
    _cameraRight = cameraTransform.right;
    _cameraRight.y = 0;
    _cameraRight.Normalize();
  }

  void Update() {
    transform.Translate(GetMovementVector() * speed * Time.deltaTime, Space.World);
  }

  protected Vector3 GetMovementVector() {
    float horizontalAxis = Input.GetAxis("Horizontal");
    float verticalAxis = Input.GetAxis("Vertical");
    Vector3 movement = _cameraRight * horizontalAxis +
     _cameraForward * verticalAxis;
    if (movement.sqrMagnitude > 1f) {
      movement.Normalize();
    }
    return movement;
  }
}
```

In this script I fix the movement relative to the camera view, so the movement is more intuitive.

![Humanoids Chasing Movement](Resources/Pr4-Ej2-chasing-target.gif)

![Humanoids Fixed Target Movement](Resources/Pr4-Ej2-fixed-target.gif)

### Exercise 3

*Adapta la escena anterior para que existan humanoides de tipo 1 y de tipo 2, así como diferentes tipos de escudos, tipo 1 y tipo 2:*

*- Cuando el cubo colisiona con cualquier humanoide tipo 2, los del grupo 1 se acercan a un escudo seleccionado. Cuando el cubo toca cualquier humanoide del grupo 1 se dirigen hacia los escudos del grupo 2 que serán objetos físicos. Si algún humanoide a colisiona con uno de ellos debe cambiar de color.*

For this exercise, I added the asset for the shield. Then I modified the resource image used by them for use multiple colors, creating two materials: one blue for type 1 shields and another purple for type 2 shields. I placed a shield behind every humanoid and another one by the mountain slope. I made prefabs for the shields and humanoids, this way I didn't need to associate them to the objets and scripts one by one. Then I added a Rigidbody component to the type 2 Shield (Purple) prefab to make them physical objects.

I created a new script called `FootmanCollisionObserver` that implements the `ICollisionObserver` interface. This script is responsible for handling the collision events for the object with CollisionNotificator attached, in this case the cube. So I managed the logic for checking the type of humanoid collided and the corresponding behavior. And use the OnCollisionEnter method to detect collisions with the type 2 shields and change the color of the humanoid using the ColorController component I had created.

```csharp
using UnityEngine;

/// <summary>
/// Controller for the Footman when cube collision is detected.
/// </summary>
public class FootmanCollisionMover : MonoBehaviour, ICollisionObserver {
  [SerializeField] private CollisionNotifier collisionNotifier;
  [SerializeField] private GameObject shield;
  [SerializeField] private float moveSpeed = 6f;

  private MovementController movement;
  private ColorController colorController;
  private string shieldTagGroup2 = "Purple Shield";

  private void Awake() {
    movement = GetComponent<MovementController>();
    colorController = GetComponent<ColorController>();
  }

  private void OnEnable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected += OnCollisionDetected;
    }
  }

  private void OnDisable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected -= OnCollisionDetected;
    }
  }

  public void OnCollisionDetected(Collision other) {
    if (other.gameObject.CompareTag("Lich")) {
      if (shield != null) {
        movement.MoveTowards(shield.transform.position, moveSpeed);
        Debug.Log($"{gameObject.name} moving to selected shield");
      }
    }
    else if (other.gameObject.CompareTag("Footman")) {
      MoveToClosestShield(shieldTagGroup2);
    }
  }

  private void MoveToClosestShield(string shieldTag) {
    GameObject[] shields = GameObject.FindGameObjectsWithTag(shieldTag);

    if (shields.Length == 0) {
      Debug.LogWarning($"No {shieldTag} found!");
      return;
    }

    GameObject closest = GetClosestShield(shields);
    if (closest != null) {
      movement.MoveTowards(closest.transform.position, moveSpeed);
      Debug.Log($"{gameObject.name} moving to closest shield: {closest.name}");
    }
  }

  private GameObject GetClosestShield(GameObject[] shields) {
    GameObject closest = null;
    float minDistance = Mathf.Infinity;

    foreach (GameObject shield in shields) {
      if (shield == null)
        continue;

      float distance = Vector3.Distance(
        transform.position,
        shield.transform.position
      );

      if (distance < minDistance) {
        minDistance = distance;
        closest = shield;
      }
    }

    return closest;
  }

  void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.CompareTag(shieldTagGroup2)) {
      if (colorController != null) {
        colorController.ChangeColorRandom();
        Debug.Log($"{gameObject.name} collided with {shieldTagGroup2} - color changed!");
      }
      else {
        Debug.LogError($"{gameObject.name} has no ColorController component!");
      }
    }
  }
}
```

Although we could have used a single script that checks for the Lich collision and moves the Footmen accordingly, so this way we don't need to attach to every Footman the script:

```csharp
using UnityEngine;

/// <summary>
/// Controller for Lich (Type 2 Humanoid)
/// When cube collides with any Lich, all Footmen move to selected shield
/// </summary>
public class LichCollisionObserver : MonoBehaviour, ICollisionObserver {
  [SerializeField] private CollisionNotifier cubeCollisionNotifier;
  [SerializeField] private GameObject selectedShield;
  [SerializeField] private float moveSpeed = 6f;

  private void OnEnable() {
    if (cubeCollisionNotifier != null) {
      cubeCollisionNotifier.OnCollisionDetected += OnCollisionDetected;
    }
  }

  private void OnDisable() {
    if (cubeCollisionNotifier != null) {
      cubeCollisionNotifier.OnCollisionDetected -= OnCollisionDetected;
    }
  }

  public void OnCollisionDetected(Collision other) {
    // Only react if cube hit a Lich (Type 2)
    if (other.gameObject.CompareTag("Lich")) {
      MoveAllFootmenToSelectedShield();
    }
  }

  private void MoveAllFootmenToSelectedShield() {
    if (selectedShield == null) {
      Debug.LogWarning("No selected shield assigned!");
      return;
    }

    GameObject[] footmen = GameObject.FindGameObjectsWithTag("Footman");

    if (footmen.Length == 0) {
      Debug.LogWarning("No Footmen found in scene!");
      return;
    }

    Debug.Log($"Lich triggered: Moving all Footmen to {selectedShield.name}");

    foreach (GameObject footman in footmen) {
      MovementController movement =
        footman.GetComponent<MovementController>();

      if (movement != null) {
        movement.MoveTowards(
          selectedShield.transform.position,
          moveSpeed
        );
        Debug.Log($"  - {footman.name} moving to selected shield");
      }
    }
  }
}
```

Color Controller Script checks for renderers in children and changes their color when requested implementing IColorable interface:

```csharp
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
```

```csharp
using UnityEngine;

/// <summary>
/// Interface for objects that can change color
/// </summary>
public interface IColorable {
  void ChangeColor(Color newColor);
  Color CurrentColor { get; }
}
```

![Humanoids Moving to Shields](Resources/Pr4-Ej3.gif)

### Excercise 4

*Cuando el cubo se aproxima al objeto de referencia, los humanoides del grupo 1 se teletransportan a un escudo objetivo que debes fijar de antemano. Los humanoides del grupo 2 se orientan hacia un objeto ubicado en la escena con ese propósito.*

For this exercise, I used the same script as before, now adding the teleportation behavior for the type 1 humanoids when the cube approaches the reference object. Adding this comprobation in the OnCollisionDetected method:

```csharp
    else if (other.gameObject.CompareTag(referenceObject.tag)) {
      transform.position = referenceObject.transform.position;
    }
```

or we could create a new script specifically for this behavior:

```csharp
using UnityEngine;

/// <summary>
/// Exercise 4: Teleports object to spawn point on collision
/// </summary>
public class CollisionTeleporter : MonoBehaviour {
  [SerializeField] private CollisionNotifier collisionNotifier;
  [SerializeField] private GameObject referenceObject;

  private void OnEnable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected += HandleCollision;
    }
  }

  private void OnDisable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected -= HandleCollision;
    }
  }

  private void HandleCollision(Collision collision) {
    if (referenceObject != null && collision.gameObject.CompareTag(referenceObject.tag)) {
      transform.position = referenceObject.transform.position;
    }
  }
}
```

For the type 2 humanoids, I created a new script that implements the ICollisionObserver interface, which orients the gameobject attached to look at a target when a collision is detected, adding a Tiger prefab to the scene as the target to look at:

```csharp
using UnityEngine;

/// <summary>
/// Exercise 4: Orients object to look at target on collision
/// </summary>
public class CollisionOrientation : MonoBehaviour {
  [SerializeField] private CollisionNotifier cubeNotifier;
  [SerializeField] private Transform targetToLookAt;
  [SerializeField] private GameObject referenceObject;

  private void OnEnable() {
    if (cubeNotifier != null) {
      cubeNotifier.OnCollisionDetected += HandleCollision;
    }
  }

  private void OnDisable() {
    if (cubeNotifier != null) {
      cubeNotifier.OnCollisionDetected -= HandleCollision;
    }
  }

  private void HandleCollision(Collision collision) {
    if (collision.gameObject.CompareTag(referenceObject.tag)) {
      if (targetToLookAt != null) {
        transform.LookAt(targetToLookAt);
      }
    }
  }
}
```

![Exercise 4 Humanoids](Resources/Pr4-Ej4.gif)

### Exercise 5

*Implementar la mecánica de recolectar escudos en la escena que actualicen la puntuación del jugador. Los escudos de tipo 1 suman 5 puntos y los de tipo 2 suman 10. Mostrar la puntuación en la consola.*

In this excersice I created a new script called `ShieldCollector` that listens for the trigger events when the cube collides with the shields. Depending on the type of shield collided, it adds the corresponding points to the score using the ScoreManager singleton ans desactivate it. I also implemented the script for CollisionNotifier to detect collisions instead of triggers, since the cube is a physical object.

```csharp
using UnityEngine;

/// <summary>
/// Exercise 5: Collects shields and updates score
/// </summary>
public class TriggerShieldCollector : MonoBehaviour {
  [SerializeField] private TriggerNotifier triggerNotifier;
  [SerializeField] private int pointsPerType1Shield = 5;
  [SerializeField] private int pointsPerType2Shield = 10;
  [SerializeField] private string type1ShieldTag = "Blue Shield";
  [SerializeField] private string type2ShieldTag = "Purple Shield";

  private void OnEnable() {
    if (triggerNotifier != null) {
      triggerNotifier.OnTriggerDetected += HandleTrigger;
    }
  }

  private void OnDisable() {
    if (triggerNotifier != null) {
      triggerNotifier.OnTriggerDetected -= HandleTrigger;
    }
  }

  private void HandleTrigger(Collider other) {
    if (other.CompareTag(type1ShieldTag) || other.CompareTag(type2ShieldTag)) {
      CollectShield(other.gameObject);
    }
  }

  private void CollectShield(GameObject shield) {
    if (ScoreManager.Instance != null) {
      if (shield.CompareTag(type1ShieldTag))
        ScoreManager.Instance.AddScore(pointsPerType1Shield);
      else if (shield.CompareTag(type2ShieldTag))
        ScoreManager.Instance.AddScore(pointsPerType2Shield);
    }
    shield.SetActive(false);
  }
}
```

Esencially the same script but using CollisionNotifier:

```csharp
using UnityEngine;

/// <summary>
/// Exercise 5: Collects shields and updates score
/// </summary>
public class ShieldCollector : MonoBehaviour {
  [SerializeField] private CollisionNotifier collisionNotifier;
  [SerializeField] private int pointsPerType1Shield = 5;
  [SerializeField] private int pointsPerType2Shield = 10;
  [SerializeField] private string type1ShieldTag = "Blue Shield";
  [SerializeField] private string type2ShieldTag = "Purple Shield";

  private void OnEnable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected += HandleCollision;
    }
  }

  private void OnDisable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected -= HandleCollision;
    }
  }

  private void HandleCollision(Collision collision) {
    if (collision.gameObject.CompareTag(type1ShieldTag) || collision.gameObject.CompareTag(type2ShieldTag)) {
      CollectShield(collision.gameObject);
    }
  }

  private void CollectShield(GameObject shield) {
    if (ScoreManager.Instance != null) {
      if (shield.CompareTag(type1ShieldTag))
        ScoreManager.Instance.AddScore(pointsPerType1Shield);
      else if (shield.CompareTag(type2ShieldTag))
        ScoreManager.Instance.AddScore(pointsPerType2Shield);
    }
    shield.SetActive(false);
  }
}
```

```csharp
using UnityEngine;

/// <summary>
/// Singleton manager for tracking and managing game score
/// </summary>
public class ScoreManager : MonoBehaviour {
  public static ScoreManager Instance { get; private set; }
  private int currentScore = 0;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else {
      Destroy(gameObject);
    }
  }

  public void AddScore(int points) {
    currentScore += points;
    Debug.Log($"Score updated: {currentScore}");
  }

  public int GetScore() {
    return currentScore;
  }
}
```

![Exercise 5 Score Collection](Resources/Pr4-Ej5.gif)

### Exercise 6

*Partiendo del script anterior crea una interfaz que muestre la puntuación que va obteniendo el cubo.*

In this one first I added a canvas and a TextMeshPro UI Objects to the scene. Making it scale with the screen size and adjusting the resolution to 1920x1080, in the TextMeshPro I set the text to "Score: 0" and the anchors to the up-left corner of the canvas. Then I updated ScoreManager for throw events whenever the score changes. At last, I created a new script called `ScoreDisplay` that subscribes to the ScoreManager's OnScoreChanged event to update the score display in the console whenever the score changes.

```csharp
using UnityEngine;

/// <summary>
/// Singleton manager for tracking and managing game score
/// </summary>
public class ScoreManager : MonoBehaviour {
  public static ScoreManager Instance { get; private set; }
  private int currentScore = 0;

  public delegate void ScoreChangedHandler(int newScore);
  public event ScoreChangedHandler OnScoreChanged;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else {
      Destroy(gameObject);
    }
  }

  public void AddScore(int points) {
    currentScore += points;
    OnScoreChanged?.Invoke(currentScore);
    Debug.Log($"Score updated: {currentScore}");
  }

  public void ResetScore() {
    currentScore = 0;
    OnScoreChanged?.Invoke(currentScore);
  }

  public int GetScore() {
    return currentScore;
  }
}
```

```csharp
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Exercise 6: Displays score in UI
/// </summary>
public class ScoreDisplay : MonoBehaviour {
  [SerializeField] private Text scoreText;

  private void OnEnable() {
    if (ScoreManager.Instance != null) {
      ScoreManager.Instance.OnScoreChanged += UpdateScoreDisplay;
      UpdateScoreDisplay(ScoreManager.Instance.GetScore());
    }
  }

  private void OnDisable() {
    if (ScoreManager.Instance != null) {
      ScoreManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
    }
  }

  private void UpdateScoreDisplay(int newScore) {
    if (scoreText != null) {
      scoreText.text = $"Score: {newScore}";
    }
  }
}
```

Working this way I had to ensure that `ScoreManager` is present in the scene before `ScoreDisplay` tries to subscribe to its event. So I edited the Script Execution Order in `Edit > Project Settings > Script Execution Order`, adding `ScoreManager` with a lower value (earlier execution) than `ScoreDisplay`. Another way to do this could be update the `ScoreManager` script directly for managing the canvas and text component.

![Exercise 6 Score Display](Resources/Pr4-Ej6.gif)

### Exercise 7

*Partiendo de los ejercicios anteriores, implementa una mecánica en la que cada 100 puntos el jugador obtenga una recompensa que se muestre en la UI.*

In this one I used the ScoreManager script from the previous exercise. I created a new script called `RewardManager` that uses the event OnScoreChanged to check when the score reaches multiples of 100 points. When that happens, a reward prefab is instantiated at a specified spawn point in the scene:

```csharp
using UnityEngine;

/// <summary>
/// Exercise 7: Manages rewards based on score thresholds
/// </summary>
public class RewardManager : MonoBehaviour {
  [SerializeField] private GameObject rewardPrefab;
  [SerializeField] private Transform spawnPoint;
  [SerializeField] private int scoreThreshold = 100;

  private void OnEnable() {
    if (ScoreManager.Instance != null) {
      ScoreManager.Instance.OnScoreChanged += CheckReward;
    }
  }

  private void OnDisable() {
    if (ScoreManager.Instance != null) {
      ScoreManager.Instance.OnScoreChanged -= CheckReward;
    }
  }

  private void CheckReward(int currentScore) {
    if (currentScore % scoreThreshold == 0 && currentScore != 0) {
      GrantReward();
    }
  }

  private void GrantReward() {
    if (rewardPrefab != null && spawnPoint != null) {
      Instantiate(rewardPrefab, spawnPoint.position, Quaternion.identity);
      Debug.Log("Reward granted!");
    }
    else {
      Debug.LogWarning("RewardPrefab or SpawnPoint is not assigned.");
    }
  }
}
```

In this case I used a Chicken prefab as the reward object, placing a spawn point behind the cube so the rewards appear there when granted. We could have granted another things as an object for the player, a power-up or pop an image in the UI.

![Exercise 7 Reward Granting](Resources/Pr4-Ej7.gif)

### Exercise 8

*Genera una escena que incluya elementos que se ajusten a la escena del prototipo y alguna de las mecánicas anteriores.*  

The prototype scene includes the used terrain, a cube as the player object, two humanoid types (Footman and Lich), and uses the two types of shields.
The cube can move using the arrow keys, the objective of the game is recollect the shields that are spawned in the scene every few seconds (up to limit of 5 shields at once), but humanoids are trying to reach them first. The Footman moves to the blue shields and the Lich to the purple ones. Blue ones give 5 points and purple ones give 10 points. The first team to reach 100 points wins the game.

The Footman have better speed than the Lich, but the Lich have better detection range for finding the shields. There are two Footman and a Lich in the scene. Also the shields spawn at random positions in the yellow zone:

![Prototype Scene Layout](Resources/Pr4prototype-scene-layout.png)

For making this posible I added to the cube the `CollisionNotifier`, and `TranslateMove` scripts as before. Then I created a new script called `ShieldSpawner` that spawns shields at random positions within a defined area at regular intervals:

```csharp
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
```

Then I created a new script called `GameScoreManager` that manages the scores of both the player and the humanoids, checking for win conditions:

```csharp
using UnityEngine;

/// <summary>
/// Singleton manager for tracking and managing game score 
/// </summary>
public class GameScoreManager : MonoBehaviour {
  public static GameScoreManager Instance { get; private set; }

  private int currentPlayerScore = 0;
  private int currentEnemyScore = 0;

  public delegate void PlayerScoreChangedHandler(int newScore);
  public event PlayerScoreChangedHandler OnPlayerScoreChanged;
  public delegate void EnemyScoreChangedHandler(int newScore);
  public event EnemyScoreChangedHandler OnEnemyScoreChanged;


  private void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else {
      Destroy(gameObject);
    }
  }

  public void AddPlayerScore(int points) {
    currentPlayerScore += points;
    OnPlayerScoreChanged?.Invoke(currentPlayerScore);
    Debug.Log($"Player score updated: {currentPlayerScore}");
  }

  public void AddEnemyScore(int points) {
    currentEnemyScore += points;
    OnEnemyScoreChanged?.Invoke(currentEnemyScore);
    Debug.Log($"Enemy score updated: {currentEnemyScore}");
  }

  public void ResetScore() {
    currentPlayerScore = 0;
    currentEnemyScore = 0;
    OnPlayerScoreChanged?.Invoke(currentPlayerScore);
    OnEnemyScoreChanged?.Invoke(currentEnemyScore);
  }

  public int GetPlayerScore() {
    return currentPlayerScore;
  }

  public int GetEnemyScore() {
    return currentEnemyScore;
  }
}
```

The `GameScoreDisplay` script subscribes to both player and enemy score change events to update the UI display:

```csharp
using TMPro;

using UnityEngine;

/// <summary>
/// Exercise 8: Displays player and enemy score in UI
/// </summary>
public class GameScoreDisplay : MonoBehaviour {
  [SerializeField] private TMP_Text playerScoreText;
  [SerializeField] private TMP_Text enemyScoreText;

  private void OnEnable() {
    if (GameScoreManager.Instance != null) {
      GameScoreManager.Instance.OnPlayerScoreChanged += UpdatePlayerScoreDisplay;
      GameScoreManager.Instance.OnEnemyScoreChanged += UpdateEnemyScoreDisplay;
      UpdatePlayerScoreDisplay(GameScoreManager.Instance.GetPlayerScore());
      UpdateEnemyScoreDisplay(GameScoreManager.Instance.GetEnemyScore());
    }
    else {
      Debug.LogWarning("GameScoreManager instance not found.");
    }
  }

  private void OnDisable() {
    if (GameScoreManager.Instance != null) {
      GameScoreManager.Instance.OnPlayerScoreChanged -= UpdatePlayerScoreDisplay;
      GameScoreManager.Instance.OnEnemyScoreChanged -= UpdateEnemyScoreDisplay;
    }
  }

  private void UpdatePlayerScoreDisplay(int newScore) {
    if (playerScoreText != null) {
      playerScoreText.text = $"Player Score: {newScore}";
    }
  }

  private void UpdateEnemyScoreDisplay(int newScore) {
    if (enemyScoreText != null) {
      enemyScoreText.text = $"Enemy Score: {newScore}";
    }
  }
}
```

The `SceneManager` script handles the win/lose conditions and UI updates accordingly:

```csharp
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
```

For the humanoids, I created a new script called `HumanoidAI` that manages their behavior, making them search for the nearest shield of their type and move towards it and collect it updating the enemy score thanks to the `GameScoreManager`:

```csharp
using UnityEngine;

/// <summary>
/// Exercise 8: Generic AI for enemies that seek and collect shields
/// Configurable to target any shield type
/// </summary>
public class HumanoidAI : MonoBehaviour {
  [Header("Target Shield")]
  [SerializeField] private string targetShieldTag = "Blue Shield";
  [SerializeField] private int pointsPerShield = 5;

  [Header("Movement Settings")]
  [SerializeField] private float moveSpeed = 5f;
  [SerializeField] private float detectionRadius = 50f;
  [SerializeField] private float collectRadius = 2f;

  private GameObject targetShield;

  private void Update() {
    if (targetShield == null || !targetShield.activeInHierarchy) {
      FindNearestShield();
    }
    if (targetShield != null) {
      MoveTowardsTarget();
      CheckShieldCollection();
    }
  }

  private void FindNearestShield() {
    GameObject[] shields = GameObject.FindGameObjectsWithTag(targetShieldTag);

    GameObject nearest = null;
    float minDistance = detectionRadius;

    foreach (GameObject shield in shields) {
      if (shield == null || !shield.activeInHierarchy)
        continue;
      float distance = Vector3.Distance(transform.position, shield.transform.position);
      if (distance < minDistance) {
        minDistance = distance;
        nearest = shield;
      }
    }
    targetShield = nearest;
  }

  private void MoveTowardsTarget() {
    if (targetShield == null)
      return;
    Vector3 direction = (targetShield.transform.position - transform.position).normalized;
    transform.position += direction * moveSpeed * Time.deltaTime;
    if (direction != Vector3.zero) {
      transform.rotation = Quaternion.LookRotation(direction);
    }
  }

  private void CheckShieldCollection() {
    if (targetShield == null)
      return;
    float distance = Vector3.Distance(transform.position, targetShield.transform.position);
    if (distance <= collectRadius) {
      CollectShield(targetShield);
    }
  }

  private void CollectShield(GameObject shield) {
    Debug.Log($"{gameObject.name} collected {targetShieldTag}! +{pointsPerShield} points");
    if (GameScoreManager.Instance != null) {
      GameScoreManager.Instance.AddEnemyScore(pointsPerShield);
    }
    shield.SetActive(false);
    targetShield = null;
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, detectionRadius);
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, collectRadius);
    if (targetShield != null) {
      Gizmos.color = Color.yellow;
      Gizmos.DrawLine(transform.position, targetShield.transform.position);
    }
  }
}
```

Finally, I modified the ShielCollector for use the GameScoreManager when the player collects shields:

```csharp
using UnityEngine;

/// <summary>
/// Exercise 5: Collects shields and updates score
/// </summary>
public class PlayerShieldCollector : MonoBehaviour {
  [SerializeField] private CollisionNotifier collisionNotifier;
  [SerializeField] private int pointsPerType1Shield = 5;
  [SerializeField] private int pointsPerType2Shield = 10;
  [SerializeField] private string type1ShieldTag = "Blue Shield";
  [SerializeField] private string type2ShieldTag = "Purple Shield";

  private void OnEnable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected += HandleCollision;
    }
  }

  private void OnDisable() {
    if (collisionNotifier != null) {
      collisionNotifier.OnCollisionDetected -= HandleCollision;
    }
  }

  private void HandleCollision(Collision collision) {
    if (collision.gameObject.CompareTag(type1ShieldTag) || collision.gameObject.CompareTag(type2ShieldTag)) {
      CollectShield(collision.gameObject);
    }
  }

  private void CollectShield(GameObject shield) {
    if (GameScoreManager.Instance != null) {
      if (shield.CompareTag(type1ShieldTag))
        GameScoreManager.Instance.AddPlayerScore(pointsPerType1Shield);
      else if (shield.CompareTag(type2ShieldTag))
        GameScoreManager.Instance.AddPlayerScore(pointsPerType2Shield);
    }
    shield.SetActive(false);
  }
}
```

![Prototype Scene](Resources/Pr4-Ej8-prototype-scene.gif)

### Exercise 9

*Implementa el ejercicio 3 siendo el cubo un objeto físico.*

I have already implemented the cube as a physical object in exercise 3, adding a Rigidbody component to it and using CollisionNotifier to detect collisions instead of TriggerNotifier. The only difference between both ways is that you need to add a Rigidbody component to the cylinder to detect collisions with it. So if you want to use TriggerNotifier instead of CollisionNotifier, you need to set the IsTrigger property of the cylinder collider to true and use isKinematic Rigidbody component in the cylinder or the rigidbody component in the cube. Otherwise, not OnTriggerEnter event will be called.
You also need to manage the movement as I have done in the previous exercises, using the MovementController component to delegate the movement logic to the IMovable implementations. with rigidbody physics you can use AddForce method. The other way you can use Transform.Translate for movement without physics. Besides, adding a slippery material to the surface with rigidbody physics makes the movement more smooth.
