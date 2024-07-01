

using UnityEngine;
using UnityEngine.UI;

public class ExpeditionButtonExtended : RightClickBehavior
{
    protected override void OnRightClick()
    {
        GameObject.Find("Expedition").SetActive(true);
        gameObject.GetComponent<Button>().onClick.Invoke();

        GameManager.i.EXPM.OpenTown();
        GameManager.i.TOMA.ClickBuilding(9);
        GameManager.i.TOMA.ClickTradeCenter();
    }
}