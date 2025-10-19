using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Image cardBackgroundImage, cardArtImage;
    [SerializeField]
    private GameObject cardSelectionRing;
    [SerializeField]
    private TMP_Text cardNameText, cardDescriptionText;

    private Vector2 savedPos, dragOffset;
    private Collider2D cardFieldCollider;
    private bool isInField;

    private void Start() {
        savedPos = transform.position;
        cardFieldCollider = CardManager.instance.FieldCollider;
        isInField = false;
        cardSelectionRing.SetActive(false);
    }

    public void SetCardNameText(string cardNameText) {
        this.cardNameText.text = cardNameText;
    }

    public void SetCardDescriptionText(string cardDescriptionText) {
        this.cardDescriptionText.text = cardDescriptionText;
    }

    public void SetCardArtImage(Image cardArtImage) {
        this.cardArtImage = cardArtImage;
    }

    private void OnMouseOver() {
        // When hovered over
        // Show the selection ring of the card
        cardSelectionRing.SetActive(true);
        // Prioritize the card in the sorting layer
        canvas.sortingOrder = 1;
    }

    private void OnMouseExit() {
        // When no longer hovered
        // Check that the card is not being dragged
        if((Vector2)transform.position == savedPos) {
            // Shide the selection ring of the card
            cardSelectionRing.SetActive(false);
            // Deprioritize the card in the sorting layer
            canvas.sortingOrder = 0;
        }
    }

    private void OnMouseDown() {
        // When about to be dragged, calculate the offset the mouse is from the center of the card
        dragOffset = savedPos - GameManager.instance.GetMousePosition();
    }

    private void OnMouseDrag() {
        // When dragging, move the card to the mouse's position, and account for an offset
        transform.position = GameManager.instance.GetMousePosition() + dragOffset;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider == cardFieldCollider) {
            Debug.Log(string.Format("{0} card over field", cardNameText.text));
            isInField = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if(collision.collider == cardFieldCollider) {
            Debug.Log(string.Format("{0} card no longer over field", cardNameText.text));
            isInField = false;
        }
    }

    private void OnMouseUpAsButton() {
        // Card being dragged
        if(isInField) {
            // If card is over the main field, play it
            Debug.Log(string.Format("{0} Played", cardNameText.text));
        } else {
            // Otherwise, move the card back to its original position
            transform.position = savedPos;
        }
    }
}
