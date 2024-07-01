


public class TownButtonExtended : RightClickBehavior
{
    protected override void OnRightClick()
    {
        if (GameManager.i.TOMA.TradeCenterBoxGO.activeSelf && GameManager.i.EXPM.TownUI.activeSelf) {
            GameManager.i.TOMA.CloseTradeCenter();
        } else {
            GameManager.i.EXPM.OpenTown();
            GameManager.i.TOMA.ClickBuilding(9);
            GameManager.i.TOMA.ClickTradeCenter();
        }
    }
}
