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
    private List<Transform> enemySpawnPositions;
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

    void Start()
    {
        enemyRounds = SetEnemyRounds();
        currentRoundNum = -1;
    }

    private List<Round> SetEnemyRounds() {
        List<Round> combatRounds = new() {
            new(new List<GameObject>() { boarEnemyPrefab }),
            new(new List<GameObject>() { mushroomEnemyPrefab, mushroomEnemyPrefab }),
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
        // Check all enemies in the scene and if none have health, the round is over
        if(GetCurrentEnemiesInScene().Where(enemy => enemy.CurrentLife > 0).ToList().Count == 0) {
            // If the round is over, end combat
            GameManager.instance.ChangeGameState(GameState.CombatEnd);
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
        switch(round.Enemies.Count) {
            case 3:
                // Spawn the first (main) enemy in the middle position
                SpawnEnemy(round.Enemies[0], enemySpawnPositions[2].position);
                // SPawn the remaining 2 enemies on the edge positions
                SpawnEnemy(round.Enemies[1], enemySpawnPositions[0].position);
                SpawnEnemy(round.Enemies[2], enemySpawnPositions[4].position);
                break;
            case 2:
                // Spawn both enemies in the second and fourth positions
                SpawnEnemy(round.Enemies[0], enemySpawnPositions[1].position);
                SpawnEnemy(round.Enemies[1], enemySpawnPositions[3].position);
                break;
            case 1:
                // Spawn the only enemy in the center spot
                SpawnEnemy(round.Enemies[0], enemySpawnPositions[2].position);
                break;
            default:
                Debug.Log(string.Format("Error! Incorrect number of enemies: {0}!", round.Enemies.Count));
                break;
        }
    }

    private void SpawnEnemy(GameObject enemy, Vector2 position) {
        GameObject newSceneEnemy = Instantiate(enemy, position, Quaternion.identity, enemies);
    }

    public List<Enemy> GetCurrentEnemiesInScene() {
        return enemies.GetComponentsInChildren<Enemy>().ToList();
    }
}
