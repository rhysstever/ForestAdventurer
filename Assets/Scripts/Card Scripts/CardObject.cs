using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    // Set in inspector
    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected GameObject cardSelectionRing;
    [SerializeField]
    private Image cardBackgroundImage, cardArtImage;
    [SerializeField]
    private TMP_Text cardNameText, cardDescriptionText;

    // Set at Start
    protected bool isBeingDragged;

    // Set in script after card is Instantiated (in DeckManager.SpawnCard())
    protected CardData cardData;

    protected virtual void Start() {
        cardSelectionRing.SetActive(false);
        isBeingDragged = false;
    }

    public void SetCardData(CardData cardData) {
        this.cardData = cardData;
        cardNameText.text = cardData.Name;
        cardDescriptionText.text = cardData.Description;

        Sprite cardArtSprite = CardManager.instance.GetCardArtSprite(cardData.Name);
        if(cardArtSprite != null) {
            cardArtImage.sprite = cardArtSprite;
        }
        // TODO: Set background image based on slot and rarity
    }

    private void OnMouseOver() {
        // When hovered over, select this card the player is not already targetting
        if(TargettingManager.instance.CardTargetting == null) {
            Select();
        }
    }

    private void OnMouseExit() {
        // When this card is first no longer hovered,
        // If this card targets, but the player is not currently targetting,
        // OR this card does not target and is not being dragged,
        // Deselect it
        if((cardData.TargetType == TargetType.Unit && TargettingManager.instance.CardTargetting == null)
            || (cardData.TargetType != TargetType.Unit && !isBeingDragged)) {
            Deselect();
        }
    }

    protected virtual void Select() {
        // Show the selection ring of the card
        cardSelectionRing.SetActive(true);
        // Prioritize the card in the sorting layer
        canvas.sortingOrder = 3;
    }

    protected virtual void Deselect() {
        // Hide the selection ring of the card
        cardSelectionRing.SetActive(false);
        // Deprioritize the card in the sorting layer
        canvas.sortingOrder = 2;
    }
}
