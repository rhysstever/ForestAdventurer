using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
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

    private int round;

    public int Round {  get { return round; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        string actionType = parsedAction[0];
        if(parsedAction[1].Contains("x")) {
            string[] damageParts = parsedAction[1].Split("x");
            for(int i = 0; i < int.Parse(damageParts[1]); i++) {
                PerformAction(actionType, int.Parse(damageParts[0]));
            }
        } else {
            PerformAction(actionType, int.Parse(parsedAction[1]));
        }
    }

    private void PerformAction(string actionType, int amount) {
        switch(actionType) {
            case "Attack":
                Attack(amount);
                break;
            case "Heal":
                Heal(amount);
                break;
            case "Defend":
                GiveDefense(amount);
                break;
            default:
                Debug.Log(string.Format("Error! No action type of {0} for enemy {1}", actionType, gameObject.name));
                break;
        }

        // Post-action updates
        IncrementRound();
        UpdateNextActionUI();
    }

    public override void TakeDamage(int amount) {
        base.TakeDamage(amount);

        if(currentLife <= 0) {
            EnemyManager.instance.CheckIfWaveIsOver();
            Destroy(gameObject);
        }
    }

    public void Attack(int amount) {
        Attack(amount, GameManager.instance.Player);
    }

    public void Attack(int amount, Unit target) {
        target.TakeDamage(amount);
    }

    public void HideNextActionUI() {
        nextActionText.gameObject.SetActive(false);
        nextActionIcon.gameObject.SetActive(false);
    }

    public void UpdateNextActionUI() {
        string[] nextAction = ParseRoundAction();
        nextActionText.text = nextAction[1];
        nextActionIcon.sprite = EnemyManager.instance.GetActionSprite(nextAction[0]);

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
