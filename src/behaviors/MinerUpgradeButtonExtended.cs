

using System.Collections;
using BreakInfinity;
using UnityEngine;
using UnityEngine.UI;

public class MinerUpgradeButtonExtended : RightClickBehavior
{
    protected override void OnRightClick()
    {
        var GM = GameManager.i;
        var MinerMain = GM.MIMA;
        if (MinerMain.CurrentMiner <= 0) {
            return;
        }

        var targets = new BigDouble[5];

        for (int ID = 0; ID < 5; ID++) {
            var ore = GM.PD.CurrentMinerals[ID];
            var ore1Percent = ore - ore.Divide(100);
            targets[ID] = ore1Percent;
        }

        while (true) {
            bool didSomething = false;
            for (int ID = 0; ID < 5; ID++) {
                if (GM.PD.CurrentMinerals[ID] < targets[ID]) {
                    continue;
                }
                MinerMain.ClickBuyUpgrade(ID);
                didSomething = true;
            }
            if (!didSomething) {
                break;
            }
        }
    }
}

public class MinerUpgradeButtonExtendedLoader : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return StartCoroutine(Utils.WaitForComponentCoroutine("Spec1ButtonBuy"));
        var targetGameObject = GameObject.Find("Spec1ButtonBuy");
        targetGameObject.AddComponent<MinerUpgradeButtonExtended>();
        Destroy(this);
    }
}