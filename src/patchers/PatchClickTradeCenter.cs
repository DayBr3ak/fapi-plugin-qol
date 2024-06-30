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

[HarmonyPatch(typeof(TownMain))]
class PatchClickTradeCenter
{
    static string EXTENDED_TEXT_ID = "PluginText1";

    [HarmonyPostfix]
    [HarmonyPatch("BuildTick")]
    static void Postfix(TownMain __instance) {
        if (__instance.TradeCenterBoxGO.activeSelf) {
            var parent = __instance.TradeCenterBoxGO;
            GameObject textObject = FindComponentByName<TextMeshProUGUI>(parent, EXTENDED_TEXT_ID);
            if (textObject != null) {
                TextMeshProUGUI textMeshProF = textObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
                textMeshProF.text = $"Time Until Full {TownCenterUtils.FormatDealTime(TownCenterUtils.GetDealTimeUntilFull())}";
                __instance.TradeCenterDealRefreshTimeText.text = $"Time Until Next {TownCenterUtils.FormatDealTime(GameManager.i.PD.TradeCenterDealTime)}";
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("ClickTradeCenter")]
    static void Postfix01(TownMain __instance)
    {
        Plugin.StaticLogger.LogInfo($"In Patch ! building Id is = {__instance.BuildingID}");
        var parent = __instance.TradeCenterBoxGO;

        // ListGraphicComponentsOfGameObject(parent);

        GameObject textObject = FindComponentByName<TextMeshProUGUI>(parent, EXTENDED_TEXT_ID);
        if (textObject == null) {
            textObject = new GameObject(EXTENDED_TEXT_ID);
            // Set the parent of the text object to the panel
            textObject.transform.SetParent(parent.transform, false);
            TextMeshProUGUI textMeshPro = textObject.AddComponent<TextMeshProUGUI>();

            var targetText = __instance.TradeCenterDealRefreshTimeText;

            // Configure the TextMeshPro component
            Utils.cloneTextMeshProUGUI(ref textMeshPro, targetText);

            // Adjust RectTransform settings if needed
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 100); // Adjust as necessary

            RectTransform rectTransformLabel = targetText.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = rectTransformLabel.anchoredPosition;
            // Position of our new added text
            rectTransform.position = new Vector3 {
                x = rectTransformLabel.position.x,
                y = rectTransformLabel.position.y - 15,
                z = rectTransformLabel.position.z
            };
            // update position of general text
            rectTransformLabel.position = new Vector3 {
                x = rectTransformLabel.position.x,
                y = rectTransformLabel.position.y + 15,
                z = rectTransformLabel.position.z
            };

            Slider timeSlider = __instance.TradeCenterDealRefreshTimeSlider;
            RectTransform sliderRectTransform = timeSlider.GetComponent<RectTransform>();
            sliderRectTransform.sizeDelta = new Vector2(sliderRectTransform.sizeDelta.x, sliderRectTransform.sizeDelta.y + 65);
        }

        TextMeshProUGUI textMeshProF = textObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
        Plugin.StaticLogger.LogInfo($"Time for a charge = {TownCenterUtils.FormatDealTime(TownCenterUtils.GetMaxDealTime())}");

        var PD = GameManager.i.PD;
        var remainingCharges = PD.TradeCenterMaxCharge - PD.TradeCenterCurrentCharge;
        Plugin.StaticLogger.LogInfo($"Time left for {remainingCharges} charges = {TownCenterUtils.FormatDealTime(TownCenterUtils.GetDealTimeUntilFull())}");

        textMeshProF.text = $"Time Until Full {TownCenterUtils.FormatDealTime(TownCenterUtils.GetDealTimeUntilFull())}";
        __instance.TradeCenterDealRefreshTimeText.text = $"Time Until Next {TownCenterUtils.FormatDealTime(PD.TradeCenterDealTime)}";
    }
}