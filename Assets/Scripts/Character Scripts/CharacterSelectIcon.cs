using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectIcon : MonoBehaviour
{
    // Set in inspector
    [SerializeField]
    private GameObject characterIconSelectedObj;
    [SerializeField]
    private SpriteRenderer characterSprite;
    [SerializeField]
    private Character character;

    void Start()
    {
        characterSprite.sprite = CharacterManager.instance.GetCharacterSprite(character);
        characterIconSelectedObj.SetActive(false);
    }

    private void OnMouseUpAsButton() {
        CharacterManager.instance.ChooseCharacter(character);
        CharacterManager.instance.HideCharacterSelectIcons();
        GameManager.instance.StartGame();
    }

    private void OnMouseEnter() {
        characterIconSelectedObj.SetActive(true);
        UIManager.instance.UpdateCharacterSelectInfo(character);
    }

    private void OnMouseExit() {
        characterIconSelectedObj.SetActive(false);
        UIManager.instance.UpdateCharacterSelectInfo();
    }
}
