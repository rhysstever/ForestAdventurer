using UnityEngine;

public class TargettingManager : MonoBehaviour
{
    // Singleton
    public static TargettingManager instance;

    // Instantiated in inspector
    [SerializeField]
    private GameObject mouseTargetAiming, mouseTarget;

    // Instantiated in script
    private GameObject cardTargetting, target;
    
    // Properties
    public GameObject CardTargetting { get { return cardTargetting; } }
    public GameObject Target { get { return target; } }

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        cardTargetting = null;
        target = null;

        mouseTargetAiming.SetActive(false);
        mouseTarget.SetActive(false);
    }

    void Update() {
        // If the player is targetting,
        if(cardTargetting != null) {
            // Enable the mouse aiming target and move it to the mouse
            mouseTargetAiming.SetActive(true);
            mouseTargetAiming.transform.position = GetMousePosition();

            // If there is a target, enable the mouse target
            mouseTarget.SetActive(target != null);

            // If enabled, move the target to the mouse
            if(mouseTarget.activeSelf) {
                mouseTarget.transform.position = GetMousePosition();
            }
        } else {
            // Otherwise, disable both targets
            mouseTargetAiming.SetActive(false);
            mouseTarget.SetActive(false);
        }
    }

    public Vector2 GetMousePosition() {
        Vector3 position = Input.mousePosition;
        position.z = -Camera.main.transform.position.z;
        return (Vector2)Camera.main.ScreenToWorldPoint(position);
    }

    public void StartTargetting(GameObject cardTargetting) {
        this.cardTargetting = cardTargetting;
    }

    public void StopTargetting() {
        cardTargetting = null;
    }

    public void SetTarget(GameObject target) {
        this.target = target;
    }
}
