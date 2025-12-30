using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CardObject : MonoBehaviour
{
    // Set in inspector
    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected GameObject cardSelectionRing, cardToBePlayedRing;
    [SerializeField]
    private Image cardArtImage, cardRarityIcon, cardSlotIcon;
    [SerializeField]
    private TMP_Text cardNameText, cardDescriptionText;

    // Set at Start
    protected bool isSelected, isBeingDragged;

    // Set in script after card is Instantiated (in DeckManager.SpawnCard())
    protected CardData cardData;

    public CardData CardData { get { return cardData; } }

    protected virtual void Start() {
        cardSelectionRing.SetActive(false);
        cardToBePlayedRing.SetActive(false);
        isSelected = false;
        isBeingDragged = false;

        Deselect();
    }

    public void SetCardData(CardData cardData) {
        this.cardData = cardData;
        cardNameText.text = cardData.Name;
        cardDescriptionText.text = cardData.Description;

        Sprite cardArtSprite = CardManager.instance.GetCardArtSprite(cardData.Name);
        if(cardArtSprite != null) {
            // Set card art image
            cardArtImage.gameObject.SetActive(true);
            cardArtImage.sprite = cardArtSprite;
        }
        else
        {
            cardArtImage.gameObject.SetActive(false);
        }

        Sprite rarityIconSprite = CardManager.instance.GetCardBaseRarityIconSprite(cardData.Rarity);
        if(rarityIconSprite != null) {
            // Set card background rarity image
            cardRarityIcon.gameObject.SetActive(true);
            cardRarityIcon.sprite = rarityIconSprite;
        }
        else
        {
            cardRarityIcon.gameObject.SetActive(false);
        }

        Sprite slotIconSprite = CardManager.instance.GetCardBaseSlotIconSprite(cardData.Slot);
        if(slotIconSprite != null)
        {
            // Set card background slot image
            cardSlotIcon.gameObject.SetActive(true);
            cardSlotIcon.sprite = slotIconSprite;
        } 
        else
        {
            cardSlotIcon.gameObject.SetActive(false);
        }
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

        isSelected = true;
    }

    protected virtual void Deselect() {
        // Hide the selection ring of the card
        cardSelectionRing.SetActive(false);
        // Deprioritize the card in the sorting layer
        canvas.sortingOrder = 2;

        isSelected = false;
    }
}
