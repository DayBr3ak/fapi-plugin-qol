using System.Collections;
using System.Linq;
using FapiQolPlugin;
using UnityEngine;
using UnityEngine.UI;

public class SortGearExtended : RightClickBehavior
{
    public Sprite Sprite { get; private set; }
    private Image image;

    private RectTransform imageRect;

    private GameObject container;

    private bool IsEnabled = false;
    private Coroutine animateCoro;
    private Coroutine updateCoro;
    private float updateInterval = 4.0f;

    public void Awake()
    {
        Sprite sprite = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(s => s.name == "AutoGear");
        if (sprite) {
            Sprite = sprite;
        } else {
            Plugin.StaticLogger.LogError("Could not find sprite");
            Destroy(this);
            return;
        }

        container = new GameObject("EquipmentSortingButtonRestartIcon");
        container.transform.SetParent(gameObject.transform, false);

        // Setup the image
        image = container.AddComponent<Image>();
        image.sprite = Sprite;

        RectTransform rectTransform = container.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2 { x = 37, y = 37 };
        rectTransform.anchoredPosition = Vector2.zero; // Center in the parent
        rectTransform.position = new Vector3 {
            x = 1745,
            y = 545,
            z = 0
        };
        imageRect = rectTransform;
        container.SetActive(false);

    }

    private IEnumerator animate() {
        while (true) {
            imageRect.localRotation *= Quaternion.Euler(0f, 0f, 100.0f * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDisable() {
        disable();
        if (updateCoro != null) {
            StopCoroutine(updateCoro);
            updateCoro = null;
        }
    }

    public void OnEnable() {
        updateCoro = StartCoroutine(MyUpdate());

        if (IsEnabled) {
            enable();
        } else {
            disable();
        }
    }

    private void enable() {
        container.SetActive(true);
        animateCoro = StartCoroutine(animate());
    }

    private void disable() {
        container.SetActive(false);
        if (animateCoro != null) {
            StopCoroutine(animateCoro);
            animateCoro = null;
        }
    }

    protected override void OnRightClick()
    {
        if (IsEnabled) {
            IsEnabled = !IsEnabled;
            disable();
        } else {
            IsEnabled = !IsEnabled;
            enable();
        }
    }

    private IEnumerator MyUpdate() {
        while (true) {
            if (IsEnabled) {
                gameObject.GetComponent<Button>().onClick.Invoke();
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }
}

public class SortGearExtendedLoader : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return StartCoroutine(Utils.WaitForComponentCoroutine("EquipmentSortingButton"));
        var targetGameObject = GameObject.Find("EquipmentSortingButton");
        if (GameManager.i.PD.SoulEquipmentSorting == 1) {
            targetGameObject.AddComponent<SortGearExtended>();
        } else {
            Plugin.StaticLogger.LogInfo("The player does not have the sorting soul upgrade");
        }
        Destroy(this);
    }
}
