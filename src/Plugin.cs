using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FapiQolPlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static ManualLogSource StaticLogger { get; set; }
    private void Awake()
    {
        // Plugin startup logic
        StaticLogger = Logger;
        var harmony = new Harmony("com.example.fapiqolplugin");
        harmony.PatchAll();

        StartCoroutine(WaitForComponentCoroutine("ButtonTown", (GameObject gameObject) => {
            gameObject.AddComponent<TownButtonExtended>();
        }));

        StartCoroutine(WaitForComponentCoroutine("ExpeditionButton", (GameObject gameObject) => {
            gameObject.AddComponent<ExpeditionButtonExtended>();
        }));
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    IEnumerator WaitForComponentCoroutine(string gameObjectName, Action<GameObject> action)
    {
        // TODO add a timeout? and an error if timeout
        GameObject targetGameObject = null;
        while (targetGameObject == null)
        {
            // Try to find the target GameObject
            targetGameObject = GameObject.Find(gameObjectName);
            yield return null; // Wait until the next frame
        }

        action(targetGameObject);
        Plugin.StaticLogger.LogDebug($"Target {gameObjectName} found.");
    }

    private void Update() {
        try {
            if (Input.GetKeyDownInt(KeyCode.F1)) {
                Logger.LogInfo("F1 Key was pressed");
                GetAllGameObjectsInScene();
            }
            if (Input.GetKeyDownInt(KeyCode.F2)) {
                return;
                Logger.LogInfo("F2 Key was pressed");

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
                    Logger.LogInfo($"Did refine all 6 equipments with {levelsToBuy}");
                    GameManager.i.E2M.InitEquipment();
                    GameManager.i.E2M.RefineValueChanged();
                    GameManager.i.E2M.UpdateTotals();
                } else {
                    Logger.LogInfo($"Not enough materials");
                }


                // GameObject targetObject =  GameObject.Find("ButtonTown");
                // if (targetObject != null)
                // {
                //     // Get the component you are interested in
                //     Component component = targetObject.GetComponent<Component>(); // Replace 'Component' with the specific type if known

                //     if (component != null)
                //     {
                //         // Get the type of the component
                //         System.Type componentType = component.GetType();

                //         // Log the type of the component
                //         Debug.Log($"The type of the component on GameObject '{targetObject.name}' is: {componentType}");

                //         // RectTransform rect = targetObject.GetComponent<RectTransform>();
                //         // targetObject.AddComponent
                //         targetObject.AddComponent<RightClickBehavior>();
                //     }
                //     else
                //     {
                //         Debug.LogError("The specified component was not found on the GameObject.");
                //     }
                // }
                // else
                // {
                //     Debug.LogError("Target GameObject not found.");
                // }
            }
            if (Input.GetKeyDownInt(KeyCode.F3)) {
                Logger.LogInfo("F3 Key was pressed");
                // Find all MonoBehaviour scripts in the scene
                MonoBehaviour[] scripts = FindObjectsOfType<MonoBehaviour>();
                Logger.LogInfo($"Found {scripts.Length} scripts");

                foreach (var script in scripts)
                {
                    Logger.LogInfo($"{script.name} {script.GetType().FullName} {script.tag} {script.gameObject.name}");

                    // if (script.gameObject.name == "ExpeditionButton") {
                    //     if (script.gameObject.GetComponent<RightClickBehavior2>() == null) {
                    //         var sc = script.gameObject.AddComponent<RightClickBehavior2>();
                    //         sc.action = () => {
                    //             GameObject.Find("Expedition").SetActive(true);
                    //             script.gameObject.GetComponent<Button>().onClick.Invoke();

                    //             GameManager.i.EXPM.OpenTown();
                    //             GameManager.i.TOMA.ClickBuilding(9);
                    //             GameManager.i.TOMA.ClickTradeCenter();
                    //         };
                    //     }
                    // }
                }
            }
        } catch (Exception ex) {
            Logger.LogError(ex);
        }
    }


    List<GameObject> GetAllGameObjectsInScene()
    {
        // List to hold all GameObjects
        List<GameObject> allGameObjects = new List<GameObject>();

        // Get the active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Get root GameObjects in the scene
        GameObject[] rootGameObjects = currentScene.GetRootGameObjects();

        // Iterate through each root GameObject
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            // Add the root GameObject and its children to the list
            AddGameObjectAndChildrenToList(rootGameObject, allGameObjects);
        }

        foreach (var go in allGameObjects)
        {
            Plugin.StaticLogger.LogInfo($"{go.name} on {go.gameObject.name}");
        }

        return allGameObjects;
    }

    void AddGameObjectAndChildrenToList(GameObject gameObject, List<GameObject> allGameObjects)
    {
        // Add the GameObject to the list
        allGameObjects.Add(gameObject);

        // Recursively add all children
        foreach (Transform child in gameObject.transform)
        {
            AddGameObjectAndChildrenToList(child.gameObject, allGameObjects);
        }
    }
}
