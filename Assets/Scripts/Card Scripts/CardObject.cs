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

    private void Start() {
        savedPos = transform.position;
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
        // When hovered over, show the selection ring of the card
        cardSelectionRing.SetActive(true);
        canvas.sortingOrder = 1;
    }

    private void OnMouseExit() {
        // When no longer hovered, hide the selection ring of the card
        // Check that the card is not being dragged
        if((Vector2)transform.position == savedPos) {
            cardSelectionRing.SetActive(false);
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

    private void OnMouseUpAsButton() {
        // If let go while being dragged, move the card back to its original position
        transform.position = savedPos;
    }
}
