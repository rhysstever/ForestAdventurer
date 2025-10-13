using UnityEngine;

public enum MenuState
{
    MainMenu,
    Game,
    GameEnd
}

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance = null;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }
        
    private MenuState currentMenuState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentMenuState = MenuState.Game;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeMenuState(MenuState newMenuState) {
        currentMenuState = newMenuState;
    }

    public Vector2 GetMousePosition() {
        Vector3 position = Input.mousePosition;
        position.z = -Camera.main.transform.position.z;
        return (Vector2)Camera.main.ScreenToWorldPoint(position);
    }
}
