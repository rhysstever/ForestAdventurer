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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardTargetting = null;
        target = null;

        mouseTargetAiming.SetActive(false);
        mouseTarget.SetActive(false);
    }

    void Update() {
        if(cardTargetting != null) {
            // Enable the mouse target if the player is targetting
            mouseTargetAiming.SetActive(true);
            mouseTargetAiming.transform.position = GetMousePosition();

            mouseTarget.SetActive(target != null);

            // If enabled, move the target to the mouse
            if(mouseTarget.activeSelf) {
                mouseTarget.transform.position = GetMousePosition();
            }
        } else {
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
