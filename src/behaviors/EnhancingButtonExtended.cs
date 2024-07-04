using System;
using System.Collections;
using ExtensionMethods;
using FapiQolPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class EnhancingButtonExtended : RightClickBehavior
{
    private GameObject confirmationBox;
    private TextMeshProUGUI boxText;
    private Button buttonYes;
    private Button buttonCancel;


    public new void Start()
    {
        base.Start();
        if (!GameManager.i.E2M.ConfirmationBox)
        {
            Plugin.StaticLogger.LogError("Cannot get reference to the PopupConfirmationBox");
            Destroy(this);
            return;
        }
        confirmationBox = Instantiate(GameManager.i.E2M.ConfirmationBox, GameManager.i.E2M.ConfirmationBox.transform.parent);
        confirmationBox.name = "PopupConfirmRefineEverything";
        try
        {
            boxText = confirmationBox.transform.FindChildByName("DeleteAllText").GetComponent<TextMeshProUGUI>();
            buttonYes = confirmationBox.transform.FindChildByName("YesDeleteAll").GetComponent<Button>();
            buttonCancel = confirmationBox.transform.FindChildByName("NoDontDelete").GetComponent<Button>();

            // the original backdrop has an extra trash all button we dont want
            var extrabtn = confirmationBox.transform.FindChildByName("ShowButton");
            if (extrabtn != null)
            {
                extrabtn.gameObject.SetActive(false);
                Destroy(extrabtn.gameObject);
            }
            else
            {
                Plugin.StaticLogger.LogInfo($"ShowButton not found");
            }
        }
        catch (Exception)
        {
            string err = "";
            if (boxText == null)
            {
                err += " DeleteAllText";
            }
            if (buttonYes == null)
            {
                err += " YesDeleteAll";
            }
            if (buttonCancel == null)
            {
                err += " NoDontDelete";
            }
            Plugin.StaticLogger.LogError($"Cannot get references to the gameObjects in PopupConfirmRefineEverything = {err}");
            Destroy(this);
            Destroy(confirmationBox);
            return;
        }

        buttonYes.onClick = new Button.ButtonClickedEvent();
        buttonYes.onClick.AddListener(() =>
        {
            InventoryUtils.RefineAllEquipment();
            confirmationBox.SetActive(false);
        });

        buttonCancel.onClick = new Button.ButtonClickedEvent();
        buttonCancel.onClick.AddListener(() =>
        {
            confirmationBox.SetActive(false);
        });
    }

    protected override void OnRightClick()
    {
        if (!confirmationBox.activeSelf)
        {
            boxText.text = $"Do you want to upgrade all your gear for {GameManager.i.PD.RefiningLevels} levels with cost:\n" + InventoryUtils.getCostText();
            confirmationBox.SetActive(true);
        }
    }
}

public class EnhancingButtonExtendedLoader : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return StartCoroutine(Utils.WaitForComponentCoroutine("ValidEnhancingButton"));
        var targetGameObject = GameObject.Find("ValidEnhancingButton");
        targetGameObject.AddComponent<EnhancingButtonExtended>();
        Destroy(this);
    }
}