using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CardActionManager : MonoBehaviour
{
    // Singleton
    public static CardActionManager instance = null;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    public void Play(string cardName) {
        Play(cardName, null);
    }

    public void Play(string cardName, GameObject target) {
        switch(cardName) {
            #region Main Hand Card Actions
            case "Shortsword":
                AttackUnit(1, target.GetComponent<Enemy>());
                break;
            case "Wand":
                AttackRandomEnemy(1);
                AttackRandomEnemy(1);
                // TODO: Add Magic to Wand Play()
                break;
            case "Longsword":
                AttackUnit(2, target.GetComponent<Enemy>());
                break;
            case "Staff":
                AttackRandomEnemy(2);
                AttackRandomEnemy(2);
                // TODO: Add Magic to Staff Play()
                break;
            case "Mace":
                AttackEveryEnemy(3);
                break;
            case "Flail":
                AttackRandomEnemy(2);
                AttackRandomEnemy(2);
                AttackRandomEnemy(2);
                break;
            case "Spear":
                AttackUnit(6, target.GetComponent<Enemy>());
                break;
            case "Trident":
                AttackUnit(4, target.GetComponent<Enemy>());
                // TODO: Add Magic to Trident Play()
                break;
            #endregion Main Hand Card Actions

            #region Off Hand Card Actions
            case "Wooden Shield":
                Defend(1);
                break;
            case "Buckler":
                Defend(2);
                break;
            case "Tome":
                Defend(0);
                // TODO: Add Magic to Tome Play()
                break;
            case "Spell Focus":
                Defend(0);
                // TODO: Add Magic to Spell Focus Play()
                break;
            case "Tower Shield":
                Defend(4);
                break;
            #endregion Off Hand Card Actions

            #region Ally Card Actions
            case "Squirrel":
                AttackUnit(1, target.GetComponent<Enemy>());
                break;
            case "Frog":
                Heal(1);
                break;
            case "Rat":
                AttackRandomEnemy(1);
                AttackRandomEnemy(1);
                AttackRandomEnemy(1);
                break;
            case "Bunny":
                Heal(2);
                break;
            case "Newt":
                AttackEveryEnemy(2);
                break;
            case "Porcupine":
                Defend(3);
                break;
            case "Hampster":
                AttackUnit(3, target.GetComponent<Enemy>());
                break;
            #endregion Ally Card Actions

            #region Spirit Card Actions
            case "Air Spirit":
                AttackRandomEnemy(1);
                AttackRandomEnemy(1);
                break;
            case "Earth Spirit":
                Defend(2);
                break;
            case "Fire Spirit":
                AttackUnit(2, target.GetComponent<Enemy>());
                break;
            case "Water Spirit":
                Heal(2);
                break;
            #endregion Spirit Card Actions

            #region Spell Card Actions
            case "Arcane Bolt":
                AttackRandomEnemy(1);
                break;
            case "Fireball":
                AttackEveryEnemy(1);
                break;
            case "Life Drain":
                AttackUnit(2, target.GetComponent<Enemy>());
                Heal(1);
                break;
            case "Lighning Bolt":
                AttackRandomEnemy(4);
                break;
            case "Heal":
                Heal(5);
                break;
            case "Blizzard":
                AttackEveryEnemy(3);
                break;
            #endregion Spell Card Actions

            // Drink Cards
            case "Cup":
                Heal(1);
                break;
            case "Pouch":
                Heal(2);
                break;
            case "Tankard":
                AttackUnit(1, target.GetComponent<Enemy>());
                Heal(1);
                break;
            case "Flask":
                AttackRandomEnemy(2);
                break;
            case "Flagon":
                AttackRandomEnemy(2);
                Heal(3);
                break;
            case "Goblet":
                Heal(4);
                break;
            case "Chalice":
                AttackUnit(1, target.GetComponent<Enemy>());
                Heal(2);
                break;
            default:
                Debug.Log(string.Format("Error! Card not found by name: {0}", cardName));
                break;
        }
    }

    #region Effects
    private void AttackUnit(int damage, Enemy enemy) {
        if(enemy == null) {
            Debug.Log("Error: No target to attack!");
            return;
        }

        if(damage < 1) {
            Debug.Log(string.Format("Error: Not enough damage ({0})", damage));
            return;
        }

        enemy.TakeDamage(damage);
    }

    private void AttackEveryEnemy(int damage) {
        GameManager.instance.GetCurrentEnemies().ForEach(enemy => {
            AttackUnit(damage, enemy);
        });
    }

    private void AttackRandomEnemy(int damage) {
        List<Enemy> currentEnemies = GameManager.instance.GetCurrentEnemies();
        int randomEnemyIndex = UnityEngine.Random.Range(0, currentEnemies.Count);
        AttackUnit(damage, currentEnemies[randomEnemyIndex]);
    }

    private void Defend(int amount) {
        DeckManager.instance.Player.GiveDefense(amount);
    }

    private void Heal(int amount) {
        DeckManager.instance.Player.Heal(amount);
    }
    #endregion Effects
}
