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
    private List<Round> enemyWaves;
    private int currentWaveNum;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        enemyWaves = SetEnemyRounds();
        currentWaveNum = -1;
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

    public void CheckIfWaveIsOver() {
        // Check all enemies in the scene and if none have health, the wave is over
        if(GetCurrentEnemiesInScene().Where(enemy => enemy.CurrentLife > 0).ToList().Count == 0) {
            // If the wave is over, end combat
            GameManager.instance.ChangeGameState(GameState.CombatEnd);
        }
    }

    public void SpawnNextWave() {
        currentWaveNum++;

        // If there are no more waves to spawn, end the game
        if(currentWaveNum >= enemyWaves.Count) {
            Debug.Log("You win! All waves defeated!");
            GameManager.instance.ChangeMenuState(MenuState.GameEnd);
            return;
        }

        Round round = enemyWaves[currentWaveNum];
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

    public bool IsLastWave() {
        return currentWaveNum == enemyWaves.Count - 1;
    }

    public List<Enemy> GetCurrentEnemiesInScene() {
        return enemies.GetComponentsInChildren<Enemy>().ToList();
    }
}
