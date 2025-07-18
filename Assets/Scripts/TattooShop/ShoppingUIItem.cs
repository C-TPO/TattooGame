using System;

using TMPro;

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

[Serializable]
[RequireComponent(typeof(Button))]
public class ShoppingUIItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI quanitityText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Image iconImage;
    [SerializeField] private CanvasGroup canvasGroup;

    private ShopItemData itemData = null;
    private Inventory inventory = null;

    private const string localizationTable = "Shop";
    private const string iconImgPath = "Images/ShopIcons/";

    #region Unity Messages

    private void OnDestroy()
    {
        buyButton.onClick.RemoveAllListeners();
    }

    #endregion

    #region Public API

    public void Init(ShopItemData itemData, Inventory inventory, Action<ShopItemData> butButtonCallback)
    {
        this.inventory = inventory;
        this.itemData = itemData;
        buyButton.onClick.AddListener(() => butButtonCallback(itemData));
        nameText.text = itemData.DisplayName;
        priceText.text = itemData.ItemCost.ToString();
        descriptionText.text = new LocalizedString(localizationTable, itemData.DescriptionKey).GetLocalizedString();
        iconImage.sprite = Resources.Load<Sprite>(iconImgPath + "ShopIcon_" + itemData.DisplayName.Replace(" ", ""));

        RefreshItem();
    }

    public void RefreshItem()
    {
        quanitityText.text = inventory.GetQuantity(itemData.ItemType).ToString();
        
        buyButton.interactable = inventory.TotalCash >= itemData.ItemCost;

        if (inventory.GetQuantity(itemData.ItemType) > 0 && itemData.IsOneTimePurchase)
        {
            buyButton.interactable = false;
            canvasGroup.alpha = .25f;
        }
    }

    #endregion
}
