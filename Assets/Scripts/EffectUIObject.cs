using TMPro;
using UnityEngine;

public class EffectUIObject : MonoBehaviour
{
    [SerializeField]
    private TMP_Text effectAmountText;
    [SerializeField]
    private SpriteRenderer effectSprite;

    public void UpdateEffectUIObject(int newAmount, Sprite newSprite) {
        effectAmountText.text = newAmount.ToString();
        effectSprite.sprite = newSprite;
    }
}
