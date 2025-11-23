using UnityEngine;

public class CardSelectionCard : CardObject
{
    protected override void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        cardToBePlayedRing.SetActive(cardData == DeckManager.instance.CurrentCardSelection);
    }

    private void OnMouseUpAsButton()
    {
        if(cardData == DeckManager.instance.CurrentCardSelection)
        {
            DeckManager.instance.SetCurrentCardSelection(null);
        } 
        else
        {
            DeckManager.instance.SetCurrentCardSelection(cardData);
        }
    }
}
