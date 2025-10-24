using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    // Set in inspector
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject cardSelectionRing, cardToBePlayedRing;
    [SerializeField]
    private Image cardBackgroundImage, cardArtImage;
    [SerializeField]
    private TMP_Text cardNameText, cardDescriptionText;

    // Set in script at Start
    private Vector2 savedPos, dragOffset, hoverOffset;
    private Collider2D cardFieldCollider;
    private bool isInField, isBeingDragged;

    // Set in script after card is Instantiated (in DeckManager.SpawnCard())
    private CardData cardData;

    private void Start() {
        savedPos = transform.position;
        dragOffset = Vector2.zero;
        hoverOffset = new Vector2(0f, 2f);
        cardFieldCollider = CardManager.instance.FieldCollider;
        isInField = false;
        isBeingDragged = false;

        cardSelectionRing.SetActive(false);
        cardToBePlayedRing.SetActive(false);
    }

    private void Update() {
        // If this card targets, enable it if this is the current targetting card and there is no target
        if(cardData.TargetType == TargetType.Unit) {
            cardToBePlayedRing.SetActive(
                TargettingManager.instance.CardTargetting == gameObject && 
                TargettingManager.instance.Target != null);
        }
    }

    public void SetCardArtImage(Image cardArtImage) {
        this.cardArtImage = cardArtImage;
    }

    public void SetCardData(CardData cardData) {
        this.cardData = cardData;
        cardNameText.text = cardData.Name;
        cardDescriptionText.text = cardData.Description;
        // TODO: Set background image based on slot

        // TODO: Set art image based on specific card
    }

    private void OnMouseEnter() {
        // When the card is first hovered over, if the player is not already targetting,
        if(TargettingManager.instance.CardTargetting == null) {
            if(cardData.TargetType == TargetType.Unit) {
                // If this card targets, move it up
                transform.position += (Vector3)hoverOffset;
            } else {
                // If this card does not target, if it is not being dragged, move it up
                if(!isBeingDragged) {
                    transform.position += (Vector3)hoverOffset;
                }
            }
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

    private void OnMouseDown() {
        // When clicked on,
        // If this card targets, start targetting it
        if(cardData.TargetType == TargetType.Unit) {
            TargettingManager.instance.StartTargetting(gameObject);
            cardToBePlayedRing.SetActive(true);
        } else {
            // If it does not target, it is about to be dragged
            // Calculate the offset the mouse is from the center of the card
            dragOffset = savedPos - TargettingManager.instance.GetMousePosition() + hoverOffset;
            isBeingDragged = true;
        }
    }

    private void OnMouseDrag() {
        // If the card does not target, drag it
        if(cardData.TargetType != TargetType.Unit) {
            // When dragging, move the card to the mouse's position, and account for an offset
            transform.position = TargettingManager.instance.GetMousePosition() + dragOffset;
        }
    }

    private void OnMouseUp() {
        // When Card is being dropped
        if(cardData.TargetType == TargetType.Unit) {
            // If the card targets and has one, play it
            if(TargettingManager.instance.CardTargetting != null && TargettingManager.instance.Target != null) {
                CardManager.instance.PlayCard(cardData.Slot, TargettingManager.instance.Target);
                Destroy(gameObject);
            } else {
                // Otherwise deselect it
                Deselect();                
                cardToBePlayedRing.SetActive(false);
            }
            TargettingManager.instance.StopTargetting();
        } else {
            // If the card does not target, check if it is in the playing field
            // If it is, play it
            if(isInField) {
                CardManager.instance.PlayCard(cardData.Slot);
                Destroy(gameObject);
            } else {
                // If not in the playing field, it should no longer be dragged
                isBeingDragged = false;
            }
        }

        // If the card is not in the playing field OR targets but didnt have one,
        // move the card back to its original position
        transform.position = savedPos;
    }

    private void Select() {
        // Show the selection ring of the card
        cardSelectionRing.SetActive(true);
        // Prioritize the card in the sorting layer
        canvas.sortingOrder = 3;
    }

    private void Deselect() {
        // Hide the selection ring of the card
        cardSelectionRing.SetActive(false);
        // Deprioritize the card in the sorting layer
        canvas.sortingOrder = 2;

        // If the card targets, stop targetting
        if(cardData.TargetType == TargetType.Unit) {
            TargettingManager.instance.StopTargetting();
        }

        // If the card is not already in its original, saved position,
        // remove the hover offset to return it to its original position
        if((Vector2)transform.position != savedPos) {
            transform.position -= (Vector3)hoverOffset;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // If the card does not target AND it has collided with the
        // field's collider, it is in the field (ready to be played)
        if(cardData.TargetType != TargetType.Unit && collision.collider == cardFieldCollider) {
            isInField = true;
            cardToBePlayedRing.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        // If the card does not target AND it has left the field's
        // collider, it is no longer in the field (cannot be played)
        if(cardData.TargetType != TargetType.Unit && collision.collider == cardFieldCollider) {
            isInField = false;
            cardToBePlayedRing.SetActive(false);
        }
    }
}
