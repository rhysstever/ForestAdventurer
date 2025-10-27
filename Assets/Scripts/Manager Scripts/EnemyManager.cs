using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Singleton
    public static EnemyManager instance = null;

    // Instantiated in inspector
    [SerializeField]
    private Transform enemies;
    [SerializeField]
    private GameObject boarEnemyPrefab, mushroomEnemyPrefab, entEnemyPrefab, undeadBoarEnemyPrefab, hagEnemyPrefab, oozeEnemyPrefab, batSwarmEnemyPrefab, zombieEnemyPrefab, shadowEnemyPrefab, necromancerEnemyPrefab;

    // Instantiated in code
    private List<Round> enemyRounds;
    private int currentRoundNum;

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
        enemyRounds = SetEnemyRounds();
        currentRoundNum = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<Round> SetEnemyRounds() {
        List<Round> combatRounds = new() {
            new(new List<GameObject>() { boarEnemyPrefab }),
            //new(new List<GameObject>() { mushroomEnemyPrefab, mushroomEnemyPrefab }),
            //new(new List<GameObject>() { entEnemyPrefab }),
            //new(new List<GameObject>() { undeadBoarEnemyPrefab, undeadBoarEnemyPrefab }),
            //new(new List<GameObject>() { hagEnemyPrefab }),
            //new(new List<GameObject>() { oozeEnemyPrefab }),
            //new(new List<GameObject>() { batSwarmEnemyPrefab, batSwarmEnemyPrefab }),
            //new(new List<GameObject>() { zombieEnemyPrefab, zombieEnemyPrefab, zombieEnemyPrefab }),
            //new(new List<GameObject>() { shadowEnemyPrefab }),
            //new(new List<GameObject>() { necromancerEnemyPrefab }),
        };

        return combatRounds;
    }

    public void CheckIfRoundIsOver() {
        if(GetCurrentEnemiesInScene().Where(enemy => enemy.CurrentLife > 0).ToList().Count == 0) {
            SpawnNextRound();
        }
    }

    public void SpawnNextRound() {
        currentRoundNum++;

        if(currentRoundNum >= enemyRounds.Count) {
            Debug.Log("You win! All rounds defeated!");
            GameManager.instance.ChangeMenuState(MenuState.GameEnd);
            return;
        }

        Round round = enemyRounds[currentRoundNum];
        for(int i = 0; i < round.Enemies.Count; i++) { 
            SpawnEnemy(round.Enemies[i]);
        }
    }

    private void SpawnEnemy(GameObject enemy) {
        GameObject newSceneEnemy = Instantiate(enemy, enemies);
    }

    public List<Enemy> GetCurrentEnemiesInScene() {
        return enemies.GetComponentsInChildren<Enemy>().ToList();
    }
}
