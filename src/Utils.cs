


using TMPro;
using UnityEngine;


using System.Collections.Generic;
using UnityEngine.UI;
using FapiQolPlugin;
using System.Reflection;

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

        Plugin.StaticLogger.LogInfo("font " + destination.font.ToString());
        Plugin.StaticLogger.LogInfo("fontSize " + destination.fontSize.ToString());
        Plugin.StaticLogger.LogInfo("color " + destination.color.ToString());
        Plugin.StaticLogger.LogInfo("alignment " + destination.alignment.ToString());
        Plugin.StaticLogger.LogInfo("fontStyle " + destination.fontStyle.ToString());
        Plugin.StaticLogger.LogInfo("fontWeight " + destination.fontWeight.ToString());
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

    public static List<Button> GetButtons(GameObject obj) {
        List<Button> buttons = new List<Button>();
        // Print all components attached to the GameObject
        Component[] components = obj.GetComponents<Component>();
        foreach (Component component in components)
        {
            Plugin.StaticLogger.LogInfo($"Component: {component.GetType().Name}");
            if (component.GetType().Name == "Button") {
                buttons.Add(component as Button);
            }
        }

        // Recursively inspect all child objects
        foreach (Transform child in obj.transform)
        {
            var r = GetButtons(child.gameObject);
            buttons.AddRange(r);
        }

        return buttons;
    }

    public static List<TextMeshProUGUI> GetTextMeshProUGUI(GameObject obj) {
        List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        // Print all components attached to the GameObject
        Component[] components = obj.GetComponents<Component>();
        foreach (Component component in components)
        {
            Plugin.StaticLogger.LogInfo($"Component: {component.GetType().Name}");
            if (component.GetType().Name == "TextMeshProUGUI") {
                texts.Add(component as TextMeshProUGUI);
            }
        }

        // Recursively inspect all child objects
        foreach (Transform child in obj.transform)
        {
            var r = GetTextMeshProUGUI(child.gameObject);
            texts.AddRange(r);
        }

        return texts;
    }

    public static void Inspect(GameObject obj)
    {
        Plugin.StaticLogger.LogInfo($"Inspecting GameObject: {obj.name}");

        // Print all components attached to the GameObject
        Component[] components = obj.GetComponents<Component>();
        foreach (Component component in components)
        {
            Plugin.StaticLogger.LogInfo($"Component: {component.GetType().Name}");
            InspectComponent(component);
        }

        // Recursively inspect all child objects
        foreach (Transform child in obj.transform)
        {
            Inspect(child.gameObject);
        }
    }

    public static void InspectComponent(Component component)
    {
        FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        PropertyInfo[] properties = component.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        // Print all fields of the component
        foreach (FieldInfo field in fields)
        {
            object value = field.GetValue(component);
            Plugin.StaticLogger.LogInfo($"  Field: {field.Name} = {value}");
        }

        // Print all properties of the component
        foreach (PropertyInfo property in properties)
        {
            if (property.CanRead)
            {
                object value;
                try
                {
                    value = property.GetValue(component, null);
                }
                catch
                {
                    value = "N/A";
                }
                Plugin.StaticLogger.LogInfo($"  Property: {property.Name} = {value}");
            }
        }
    }

    public static Dictionary<string, TMP_FontAsset> _fontCache = new Dictionary<string, TMP_FontAsset>();
    public static TMP_FontAsset GetTMP_FontAsset(string path) {
        if (_fontCache.ContainsKey(path)) {
            return _fontCache[path];
        }

        var fonts = UnityEngine.Object.FindObjectsOfType<TMP_FontAsset>();
        TMP_FontAsset targetFont = null;
        foreach (TMP_FontAsset font in fonts) {
            if (font.name == path) {
                targetFont = font;
                break;
            }
        }

        if (targetFont != null) {
            _fontCache.Add(path, targetFont);
        }
        return targetFont;
    }
}