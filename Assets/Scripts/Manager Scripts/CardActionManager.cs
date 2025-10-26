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
            // Main Hand Cards
            case "Shortsword":
                Attack(1, target.GetComponent<Enemy>());
                break;
            case "Longsword":
                Attack(2, target.GetComponent<Enemy>());
                break;
            // Off Hand Cards
            case "Wooden Shield":
                Defend(1);
                break;
            // Spell Cards
            case "Arcane Bolt":
                Attack(1, target.GetComponent<Enemy>());
                break;
            // Drink Cards
            case "Cup":
                Heal(1);
                break;
            default:
                Debug.Log(string.Format("Error! Card not found by name: {0}", cardName));
                break;
        }
    }

    #region Effects
    private void Attack(int damage, Enemy enemy) {
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

    private void Defend(int amount) {
        DeckManager.instance.Player.GiveDefense(amount);
    }

    private void Heal(int amount) {
        DeckManager.instance.Player.Heal(amount);
    }

    private void AttackSpell(int damage, Enemy enemy) {
        Debug.Log(string.Format("Damage: {0} to {1}", damage, enemy.name));
    }
    #endregion Effects
}
