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

    private void OnMouseOver()
    {
        // When hovered over, select this card the player is not already targetting
        if(TargettingManager.instance.CardTargetting == null)
        {
            Select();
        }
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
