using System;
using System.Collections;
using System.Reflection;
using FapiQolPlugin;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RightClickBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{

    public Action action;
    private Image img;
    private Button button;

    void Start()
    {
        Plugin.StaticLogger.LogInfo($"RightClickBehavior attached and running on {gameObject.name}.");
        button = gameObject.GetComponent<Button>();
        img = gameObject.GetComponent<Image>();
    }

    // Method to be called when the right mouse button is clicked
    private void OnRightClick()
    {
        Plugin.StaticLogger.LogInfo($"Right-click detected on {gameObject.name}!");
        if (action != null) {
            EventSystem.current.SetSelectedGameObject(gameObject);
            action();
        } else {
            Plugin.StaticLogger.LogInfo("Nothing to do");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) {
            if (img != null && button != null) {
                img.color = button.colors.pressedColor;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) {
            if (img != null && button != null) {
                img.color = button.colors.normalColor;
            }
            OnRightClick();
        }
    }
}
