


using TMPro;
using UnityEngine;


using System.Collections.Generic;
using UnityEngine.UI;
using FapiQolPlugin;

public class Utils {
    public static void cloneTextMeshProUGUI(ref TextMeshProUGUI destination, TextMeshProUGUI other) {
        destination.text = other.text;
        destination.font = other.font;
        destination.fontSize = other.fontSize;
        destination.color = other.color;
        destination.alignment = other.alignment;
        destination.enableWordWrapping = other.enableWordWrapping;
        destination.richText = other.richText;
        destination.overflowMode = other.overflowMode;
        destination.margin = other.margin;
        destination.fontStyle = other.fontStyle;
        destination.fontWeight = other.fontWeight;
    }

    public static GameObject FindComponentByName<T>(GameObject gameObject, string cId) where T: Component {
        List<Component> graphicComponents = new List<Component>();
        T[] textMeshProUGUIs = gameObject.GetComponentsInChildren<T>();
        graphicComponents.AddRange(textMeshProUGUIs);
        foreach (var component in graphicComponents) {
            if (component.gameObject.name == cId) {
                return component.gameObject;
            }
        }
        return null;
    }
    public static void ListGraphicComponentsOfGameObject(GameObject gameObject)
    {
        // List to hold all graphic components
        List<Component> graphicComponents = new List<Component>();

        // Add Renderer components
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        graphicComponents.AddRange(renderers);

        // Add UI Image components
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        graphicComponents.AddRange(images);

        // Add UI Text components
        Text[] texts = gameObject.GetComponentsInChildren<Text>();
        graphicComponents.AddRange(texts);

        // Add TextMeshProUGUI components
        TextMeshProUGUI[] textMeshProUGUIs = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        graphicComponents.AddRange(textMeshProUGUIs);

        // Log all graphic components
        Plugin.StaticLogger.LogInfo($"Graphic components in {gameObject.name}:");
        foreach (var component in graphicComponents)
        {
            Plugin.StaticLogger.LogInfo($"{component.GetType().Name} on {component.gameObject.name}");
        }
    }
}