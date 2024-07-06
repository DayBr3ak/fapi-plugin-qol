using BepInEx.Logging;
using FapiQolPlugin;
using HarmonyLib;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.Reflection;

using static Utils;
using System.Collections;

[HarmonyPatch(typeof(Inventory2Main))]
class PatchInventoryFilterSlots
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Inventory2Main.InitEquipment))]
    static void PostfixInitEquipment(Inventory2Main __instance)
    {
        // Plugin.StaticLogger.LogInfo("Prefix PostfixInitEquipment");
        GameManager.i.PD.ItemFilterSlotCount = 6;
    }

    /**
    ** This Patch is replacing the original function
    */
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Inventory2Main.UpdateItemFilterSlots))]
    static bool PrefixUpdateItemFilterSlots(Inventory2Main __instance, ItemData newItem)
    {
        // Plugin.StaticLogger.LogInfo("Prefix PatchInventoryFilterSlots");
        var GM = GameManager.i;
        if (newItem == null) {
            return true;
        }
        List<ItemData> inventoryItems = GM.PD.InventoryItems;
        newItem.UpdateItemRating();
        ItemData itemData = newItem;
        int num = -1;

        // Start PATCH
        // We want to replace in the first slot, the sword, with a better sword if better item rating
        // repeat for other slots
        // order is 0 = sword, 1 = head, 2 = shield, 3 = gloves, 4 = torso, 5 = boots

        for (int i = 0; i < GM.PD.ItemFilterSlotCount; i++) {
            ItemData itemData2 = inventoryItems[i];
            if (itemData2.Locked == 0
            && (itemData2.ItemRarity < itemData.ItemRarity || (itemData2.ItemRarity == itemData.ItemRarity && itemData2.BaseItemRating < itemData.BaseItemRating)))
            {
                if (itemData.ItemType == i) {
                    itemData = itemData2;
                    num = i;
                }
            }
        }

        // END PATCH

        if (itemData.ItemRarity < GM.BOF.MinRarity())
        {
            GM.E2M.AddRefineMaterial(itemData.ItemRarity, GM.PD.EnhancingMaterialQtyD);
            GM.ACM.TotalEnhancingMaterial(GM.PD.EnhancingMaterialQtyD);
        }
        else if (GM.PD.SoulAutoRecycleEquipmentFull > 0)
        {
            GM.E2M.AddRefineMaterial(itemData.ItemRarity, GM.PD.EnhancingMaterialQtyD * (0.25 + (double)GM.PD.SoulReclaimer * 0.25 + (double)GM.PD.WAPAutoScrapperMaterial * 0.05));
            GM.ACM.TotalEnhancingMaterial(GM.PD.EnhancingMaterialQtyD * (0.25 + (double)GM.PD.SoulReclaimer * 0.25 + (double)GM.PD.WAPAutoScrapperMaterial * 0.05));
        }
        if (num >= 0)
        {
            newItem.UpdateBonuses();
            __instance.StartCoroutine(DelayScrappableState(newItem));
            inventoryItems[num] = newItem;
            InventoryItemController inventoryItemController;
            if (__instance.InventoryHolderGO[0].transform.GetChild(num).gameObject.TryGetComponent<InventoryItemController>(out inventoryItemController))
            {
                if (__instance.EnhancingItem.enhancingIIC == inventoryItemController)
                {
                    __instance.EnhancingItem.Clear();
                }
                else if (__instance.SelectedItem == inventoryItemController)
                {
                    __instance.UnselectItem();
                }
                inventoryItemController.ItemData = newItem;
            }
        }

        return false;
    }

    private static IEnumerator DelayScrappableState(ItemData item) {
        if (item != null)
        {
            item.CanScrap = false;
            yield return new WaitForSeconds(1f);
            item.CanScrap = true;
        }
    }
}