using System.Reflection;
using FapiQolPlugin;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickBehavior : MonoBehaviour, IPointerClickHandler
{

    void Start()
    {
        Plugin.StaticLogger.LogInfo("RightClickBehavior attached and running.");
    }

    // This method is called when the user clicks on the UI element
    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the right mouse button was clicked
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Call your method when the right mouse button is clicked
            OnRightClick();
        }
    }

    // Method to be called when the right mouse button is clicked
    private void OnRightClick()
    {
        Plugin.StaticLogger.LogInfo("Right-click detected on ButtonTown!");
        if (GameManager.i.TOMA.TradeCenterBoxGO.activeSelf) {
            GameManager.i.TOMA.CloseTradeCenter();
        } else {
            GameManager.i.EXPM.OpenTown();
            GameManager.i.TOMA.ClickBuilding(9);
            GameManager.i.TOMA.ClickTradeCenter();
        }
    }
}
