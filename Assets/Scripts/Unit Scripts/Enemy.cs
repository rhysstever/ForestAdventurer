using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Unit
{
    [SerializeField]
    private TMP_Text nextActionText;
    [SerializeField]
    private Image nextActionIcon;
    [SerializeField]
    private List<string> actions;
    [SerializeField]
    private GameObject enemySummonPrefab;

    private int round;

    public int Round {  get { return round; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        Reset();
        UpdateNextActionUI();
    }

    public override void Reset() {
        base.Reset();
        round = 0;
    }

    public void IncrementRound() {
        round++;
    }

    private string[] ParseRoundAction() {
        int actionRound = round % actions.Count;
        return actions[actionRound].Split(" ");
    }

    public void PerformRoundAction() {
        string[] parsedAction = ParseRoundAction();
        string actionString = parsedAction[0];
        if(parsedAction[1].Contains("x")) {
            string[] damageParts = parsedAction[1].Split("x");
            for(int i = 0; i < int.Parse(damageParts[1]); i++) {
                PerformAction(actionString, int.Parse(damageParts[0]));
            }
        } else {
            PerformAction(actionString, int.Parse(parsedAction[1]));
        }
    }

    private void PerformAction(string actionString, int amount) {

        switch(actionString.Split(" ")[0]) {
            case "Attack":
                GameManager.instance.Player.TakeDamage(amount, this, true);
                break;
            case "Heal":
                Heal(amount);
                break;
            case "Defend":
                GiveDefense(amount);
                break;
            case "Burn":
                GameManager.instance.Player.GiveBurn(amount);
                break;
            case "Poison":
                GameManager.instance.Player.GivePoison(amount);
                break;
            case "Summon":
                int enemyCount = int.Parse(actionString.Split(" ")[1]);

                if(enemySummonPrefab == null) {
                    Debug.Log(string.Format("Error! No enemy summon prefab found!"));
                    break;
                } 
                else
                {
                    for(int i = 0; i < enemyCount; i++)
                    {
                        EnemyManager.instance.SpawnSummon(enemySummonPrefab);
                    }
                }
                break;
            default:
                Debug.Log(string.Format("Error! No action type of {0} for enemy {1}", actionString, gameObject.name));
                break;
        }

        // Post-action updates
        IncrementRound();
        UpdateNextActionUI();
    }

    public override void TakeDamage(int amount) { 
        TakeDamage(amount, null, true);
    }

    public override void TakeDamage(int amount, Unit attacker, bool isDamageBlockable) {
        base.TakeDamage(amount, attacker, isDamageBlockable);

        if(currentLife <= 0) {
            EnemyManager.instance.CheckIfWaveIsOver();
            Destroy(gameObject);
        }
    }

    public void HideNextActionUI() {
        nextActionText.gameObject.SetActive(false);
        nextActionIcon.gameObject.SetActive(false);
    }

    public void UpdateNextActionUI() {
        string[] nextAction = ParseRoundAction();
        nextActionText.text = nextAction[1];
        nextActionIcon.sprite = CardManager.instance.GetActionSprite(nextAction[0]);

        nextActionText.gameObject.SetActive(true);
        nextActionIcon.gameObject.SetActive(true);
    }

    private void OnMouseEnter() {
        // If the player is currently targetting, set this enemy as the target
        if(TargettingManager.instance.CardTargetting != null) {
            TargettingManager.instance.SetTarget(gameObject);
        }
    }

    private void OnMouseExit() {
        // If the player is currently targetting, remove this enemy as the target
        if(TargettingManager.instance.CardTargetting != null) {
            TargettingManager.instance.SetTarget(null);
        }
    }
}
