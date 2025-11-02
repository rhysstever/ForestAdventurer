using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton
    private static UIManager instance;

    [SerializeField]
    private Button endTurnButton;

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
        endTurnButton.onClick.AddListener(() => GameManager.instance.ChangeGameState(GameState.CombatEnemyTurn));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
