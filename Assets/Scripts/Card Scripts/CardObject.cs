using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    [SerializeField]
    private Image cardBackgroundImage, cardImage;
    [SerializeField]
    private TMP_Text cardNameText, cardDescriptionText;

    private Vector2 savedPos, dragOffset;

    private void Start() {
        savedPos = transform.position;
    }

    public void SetCardNameText(string cardNameText) {
        this.cardNameText.text = cardNameText;
    }

    public void SetCardDescriptionText(string cardDescriptionText) {
        this.cardDescriptionText.text = cardDescriptionText;
    }

    public void SetCardImage(Image cardImage) {
        this.cardImage = cardImage;
    }

    private void OnMouseOver() {
        // When hovered over, highlight the card
        cardBackgroundImage.color = Color.red;
    }

    private void OnMouseExit() {
        // When no longer hovered, un-highlight the card
        cardBackgroundImage.color = Color.white;
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
