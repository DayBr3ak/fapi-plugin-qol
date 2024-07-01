using System;
using System.Collections;
using System.Reflection;
using FapiQolPlugin;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class RightClickBehavior : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
    private Image img;
    private Button button;

    void Start()
    {
        Plugin.StaticLogger.LogInfo($"RightClickBehavior attached and running on {gameObject.name}.");
        button = gameObject.GetComponent<Button>();
        img = gameObject.GetComponent<Image>();
    }

    // Method to be called when the right mouse button is clicked
    private void _OnRightClick()
    {
        Plugin.StaticLogger.LogInfo($"Right-click detected on {gameObject.name}!");
        EventSystem.current.SetSelectedGameObject(gameObject);
        OnRightClick();
    }

    protected abstract void OnRightClick();
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
            _OnRightClick();
        }
    }
}
