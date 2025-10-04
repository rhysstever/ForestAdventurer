using NUnit.Framework;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance = null;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    // Slots
    private MainHand mainHand;
    private OffHand offHand;
    private Spirit spirit;
    private Ally ally;
    private Spell spell;
    private Drink drink;
    // TODO: Add in passive slots
    //private GameObject hat;
    //private GameObject boots;

    // Stats
    private int maxLife, currentLife, currentGold, currentDefense;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage) {
        currentLife -= damage;

        if(currentLife <= 0) {
            GameManager.instance.ChangeMenuState(MenuState.GameEnd);
        }
    }

    public void Reset() {
        maxLife = 10;
        currentLife = maxLife;
        currentGold = 0;
        currentDefense = 0;

        // Setup starter slots
        mainHand = CardManager.instance.GetStarterCard(Slot.MainHand) as MainHand;
        offHand = CardManager.instance.GetStarterCard(Slot.OffHand) as OffHand;
        ally = CardManager.instance.GetStarterCard(Slot.Ally) as Ally;
        spirit = CardManager.instance.GetStarterCard(Slot.Spirit) as Spirit;
        spell = CardManager.instance.GetStarterCard(Slot.Spell) as Spell;
        drink = CardManager.instance.GetStarterCard(Slot.Drink) as Drink;
    }
}
