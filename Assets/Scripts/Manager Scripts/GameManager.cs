using System.Collections.Generic;
using System.Linq;
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

    // Instantiated in inspector
    [SerializeField]
    private Transform enemies;

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

    public List<Enemy> GetCurrentEnemies() {
        return enemies.GetComponentsInChildren<Enemy>().ToList();
        ;
    }
}
