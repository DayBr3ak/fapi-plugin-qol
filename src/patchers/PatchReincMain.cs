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

[HarmonyPatch(typeof(ReincarnationMain))]
class PatchReincMainUpdate
{
    [HarmonyTargetMethod]
    static MethodBase TargetMethod() {
        return typeof(ReincarnationMain)
            .GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    [HarmonyPostfix]
    static void Postfix(ReincarnationMain __instance)
    {
        if (!GameManager.i.OCF.Reincarnation.activeSelf) {
            // Reinc tab is not open
            return;
        }
        var GM = GameManager.i;
        // I want to modify the slider to represent the progress to ascension goal
        var progress = __instance.NextReincLvl - GM.PD.ReincarnationLevel;
        var goalRemaining = GM.PD.AscensionReincLevelRequired - GM.PD.ReincarnationLevel;
        var progressRatio = progress / (double)goalRemaining;

        __instance.ReincarnationExpSlider.value = (float)progressRatio;
        __instance.ReincarnationExpText.text = $"{Form.Numb((double)progress, 12, 2, 0, 3)}/{Form.Numb((double)goalRemaining, 12, 2, 0, 3)}";

        if (progressRatio >= 1.0) {
            // goal is reached
            __instance.ReincarnationExpText.color = Color.green;
        } else {
            __instance.ReincarnationExpText.color = Color.white;
        }
    }
}