

using System.Collections.Generic;
using System.Linq;
using FapiQolPlugin;

public class InventoryUtils {

    public static string getCostText() {
        ItemData[] items = GameManager.i.PD.EquippedItems;
        double levelsToBuy =  GameManager.i.PD.RefiningLevels;
        Dictionary<int, double> costMap = new Dictionary<int, double>();

        foreach (var item in items) {
            // Logger.LogInfo($"{item.ItemName}: {item.AreaDropped}");
            double cost = GameManager.i.E2M.RefineCost(item, levelsToBuy);
            if (costMap.ContainsKey(item.ItemRarity)) {
                costMap[item.ItemRarity] += cost;
            } else {
                costMap.Add(item.ItemRarity, cost);
            }
        }

        List<string> texts = new List<string>();
        foreach (var pair in costMap) {
            texts.Add($"{Form.Numb((double)pair.Value, 12, 2, 0, 3)} T{pair.Key} materials");
        }
        return string.Join("\n", texts.ToArray());
    }
    public static void RefineAllEquipment() {
        ItemData[] items = GameManager.i.PD.EquippedItems;
        double levelsToBuy =  GameManager.i.PD.RefiningLevels;
        Dictionary<int, double> costMap = new Dictionary<int, double>();

        foreach (var item in items) {
            // Logger.LogInfo($"{item.ItemName}: {item.AreaDropped}");
            double cost = GameManager.i.E2M.RefineCost(item, levelsToBuy);
            if (costMap.ContainsKey(item.ItemRarity)) {
                costMap[item.ItemRarity] += cost;
            } else {
                costMap.Add(item.ItemRarity, cost);
            }
        }

        bool canRefine = true;
        foreach (var pair in costMap) {
            if(GameManager.i.E2M.GetRefineMaterial(pair.Key) < pair.Value) {
                canRefine = false;
                break;
            }
        }
        if (canRefine) {
            foreach (var item in items) {
                double cost = GameManager.i.E2M.RefineCost(item, levelsToBuy);
                item.RefineLevel += levelsToBuy;
                item.UpdateBonuses();
                GameManager.i.E2M.AddRefineMaterial(item.ItemRarity, -cost);
            }
            GameManager.i.ADM.PlayClickEnhancing();
            Plugin.StaticLogger.LogInfo($"Did refine all 6 equipments with {levelsToBuy}");
            GameManager.i.E2M.InitEquipment();
            GameManager.i.E2M.RefineValueChanged();
            GameManager.i.E2M.UpdateTotals();
        } else {
            Plugin.StaticLogger.LogInfo($"Not enough materials");
        }
    }
}