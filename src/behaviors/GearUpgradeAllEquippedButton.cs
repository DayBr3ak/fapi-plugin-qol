using FapiQolPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearUpgradeAllEquippedButton : MonoBehaviour
{
    public Button originalButton; // Reference to the original button
    public Vector2 offset = new Vector2(100, 0); // Offset for the duplicated button

    void Start()
    {
        // Check if the original button is assigned
        if (originalButton == null)
        {
            Debug.LogError("Original button is not assigned.");
            return;
        }

        // var originalImage = originalButton.gameObject.GetComponent<Image>();
        // var duplicatedImage = Instantiate(originalImage, originalImage.transform.parent);

        GameObject buttonObject = new GameObject();
        buttonObject.transform.SetParent(originalButton.transform.parent);

        // Add RectTransform component
        RectTransform originalRectTransform = originalButton.GetComponent<RectTransform>();
        RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(originalRectTransform.sizeDelta.x, 30);
        rectTransform.anchoredPosition = originalRectTransform.anchoredPosition + offset;
        rectTransform.Rotate(0, 0, 180);

        // Add Image component and set the button's background
        Image image = buttonObject.AddComponent<Image>();
        image.color = Color.white; // Set the background color of the button

        // Create a new GameObject for the button's text
        GameObject textObject = new GameObject();
        textObject.transform.SetParent(buttonObject.transform);

        TextMeshProUGUI textMeshPro = textObject.AddComponent<TextMeshProUGUI>();

        // Add RectTransform component to the text object
        RectTransform textRectTransform = textObject.GetComponent<RectTransform>();
        textRectTransform.sizeDelta = rectTransform.sizeDelta; // Set the size of the text to match the button
        textRectTransform.anchoredPosition = new Vector2(0, 0); // Set the position of the text

        // TODO find how to setup a font
        textMeshPro.text = "Upgrade Equipped Gear"; // Set the text of the button

        TMP_FontAsset targetFont = Utils.GetTMP_FontAsset("1MainBlackOutlined");
        if (targetFont != null) {
            textMeshPro.font = targetFont; // Set the font of the text
        }
        textMeshPro.fontSize = 17;
        textMeshPro.fontStyle = FontStyles.Bold;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = Color.black; // Set the text color

        // GameManager.i.E2M.ConfirmationText.text = "Do you want to upgrade all your gear at once?";
        var confirmationBoxClone = Instantiate(GameManager.i.E2M.ConfirmationBox, GameManager.i.E2M.ConfirmationBox.transform.parent);
        var boxButtons = Utils.GetButtons(confirmationBoxClone);
        Plugin.StaticLogger.LogInfo($"Found {boxButtons.Count} buttons");
        foreach (var b in boxButtons) {
            if (b.name == "YesDeleteAll") {
                b.onClick = new Button.ButtonClickedEvent();
                b.onClick.AddListener(() => {
                    InventoryUtils.RefineAllEquipment();
                    confirmationBoxClone.SetActive(false);
                });
            }
            if (b.name == "NoDontDelete") {
                b.onClick = new Button.ButtonClickedEvent();
                b.onClick.AddListener(() => {
                    confirmationBoxClone.SetActive(false);
                });
            }
        }

        var innerTexts = Utils.GetTextMeshProUGUI(confirmationBoxClone);

        // Add Button component
        Button button = buttonObject.AddComponent<Button>();
        button.onClick.AddListener(() => {
            if (!confirmationBoxClone.activeSelf) {
                foreach (var text in innerTexts) {
                    text.text = $"Do you want to upgrade all your gear for {GameManager.i.PD.RefiningLevels} levels with cost:\n" + InventoryUtils.getCostText() ;
                }
                confirmationBoxClone.SetActive(true);
                // Utils.Inspect(xx);
            }
        });

    }
}
