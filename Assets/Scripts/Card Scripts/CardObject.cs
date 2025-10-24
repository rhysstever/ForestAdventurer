using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    // Set in inspector
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject cardSelectionRing;
    [SerializeField]
    private Image cardBackgroundImage, cardArtImage;
    [SerializeField]
    private TMP_Text cardNameText, cardDescriptionText;

    // Set in script at Start
    private Vector2 savedPos, dragOffset, hoverOffset;
    private Collider2D cardFieldCollider;
    private bool isInField, isBeingDragged, readyToBePlayed;

    // Set in script after card is Instantiated (in DeckManager.SpawnCard())
    private Card cardData;

    private void Start() {
        savedPos = transform.position;
        dragOffset = Vector2.zero;
        hoverOffset = new Vector2(0f, 2f);
        cardFieldCollider = CardManager.instance.FieldCollider;
        isInField = false;
        isBeingDragged = false;

        cardSelectionRing.SetActive(false);
    }

    public void SetCardArtImage(Image cardArtImage) {
        this.cardArtImage = cardArtImage;
    }

    public void SetCardData(Card cardData) {
        this.cardData = cardData;
        cardNameText.text = cardData.Name;
        cardDescriptionText.text = cardData.Description;
        // TODO: Set background image based on slot

        // TODO: Set art image based on specific card
    }

    private void OnMouseOver() {
        // When hovered over, select the card
        Select();
    }

    private void OnMouseEnter() {
        // When the card is first hovered over,
        // If it is not being dragged, move it up
        if(!isBeingDragged) {
            transform.position += (Vector3)hoverOffset;
        }
    }

    private void OnMouseExit() {
        // When the card is first no longer hovered,
        // If the card targets, but is not currently targetting,
        // OR the card does not target and is not being dragged,
        // Deselect it
        if((cardData.TargetType == TargetType.Unit && !DeckManager.instance.IsTargetting)
            || (cardData.TargetType != TargetType.Unit && !isBeingDragged)) {
            Deselect();
        }
    }

    private void OnMouseDown() {
        // If the clicked on card targets, start targetting
        if(cardData.TargetType == TargetType.Unit) {
            DeckManager.instance.StartTargetting();
        } else {
            // If it does not target, it is about to be dragged
            // Calculate the offset the mouse is from the center of the card
            dragOffset = savedPos - GameManager.instance.GetMousePosition() + hoverOffset;
            isBeingDragged = true;
        }
    }

    private void OnMouseDrag() {
        // If the card does not target, drag it
        if(cardData.TargetType != TargetType.Unit) {
            // When dragging, move the card to the mouse's position, and account for an offset
            transform.position = GameManager.instance.GetMousePosition() + dragOffset;
        }
    }

    private void OnMouseUp() {
        // When Card is being dropped
        if(cardData.TargetType == TargetType.Unit) {
            // If the card needs a target and has one, play it
            if(DeckManager.instance.IsTargetting && DeckManager.instance.Target != null) {
                CardManager.instance.PlayCard(cardData.Slot, DeckManager.instance.Target);
                Destroy(gameObject);
            } else {
                // Otherwise deselect it
                Deselect();
            }
        } else {
            // If the card does not need a target, check if it is ready to be played
            // and play it if its ready
            if(readyToBePlayed) {
                CardManager.instance.PlayCard(cardData.Slot);
                Destroy(gameObject);
            } else {
                // If not ready, it should no longer be dragged
                isBeingDragged = false;
            }
        }

        // If the card is not in the main field or required a target but didnt have one,
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

        // If the card needs a target, stop targetting
        if(cardData.TargetType == TargetType.Unit) {
            DeckManager.instance.StopTargetting();
        }

        // If the card is not already in its original, saved position,
        // remove the hover offset to return it to its original position
        if((Vector2)transform.position != savedPos) {
            transform.position -= (Vector3)hoverOffset;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // If the card does not need a target AND it has collided
        // with the field's collider, it is in the field and is ready to be played
        if(cardData.TargetType != TargetType.Unit && collision.collider == cardFieldCollider) {
            isInField = true;
            readyToBePlayed = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        // If the card does not need a target AND it has left
        // the field's collider, it is no longer in the field and cannot be played
        if(cardData.TargetType != TargetType.Unit && collision.collider == cardFieldCollider) {
            isInField = false;
            readyToBePlayed = false;
        }
    }
}
