using TMPro;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    [SerializeField]
    private TMP_Text cardNameText, cardDescriptionText;
    [SerializeField]
    private GameObject cardImageObject;

    public void SetCardNameText(string cardNameText) {
        this.cardNameText.text = cardNameText;
    }

    public void SetCardDescriptionText(string cardDescriptionText) {
        this.cardDescriptionText.text = cardDescriptionText;
    }

    public void SetCardImage(GameObject cardImageObject) {
        this.cardImageObject = cardImageObject;
    }
}
