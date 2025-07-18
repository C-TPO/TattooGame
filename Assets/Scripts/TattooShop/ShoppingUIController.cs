using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShoppingUIController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject container = null;
    [SerializeField] private RectTransform shopItemParent = null;
    [SerializeField] private GameObject shopItemUIPrefab = null;
    [SerializeField] private GenericPopupController genericPopupController = null;
    [SerializeField] private TextMeshProUGUI moneyText = null;

    private List<ShoppingUIItem> shopItems = new List<ShoppingUIItem>();
    private Inventory inventory = null;

    #region Unity Messages

    private void Start()
    {
        RefreshCashUI();
        DataPersistenceManager.instance.GameData.inventory.OnCashChanged += RefreshCashUI;
    }

    private void OnDestroy()
    {
        DataPersistenceManager.instance.GameData.inventory.OnCashChanged -= RefreshCashUI;
    }

    #endregion

    #region Public API

    public void Show()
    {
        InitShopItems();
        container.SetActive(true);
    }

    public void Hide()
    {
        container.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        inventory = data.inventory;
    }

    public void SaveData(GameData data)
    {
        data.inventory = inventory;
    }

    #endregion

    #region Implementation

    private void RefreshCashUI()
    {
        moneyText.text = DataPersistenceManager.instance.GameData.inventory.TotalCash.ToString();
    }

    private void InitShopItems()
    {
        if (shopItemParent.childCount != 0 || inventory == null)
            return;

        foreach (var item in inventory.GetShopItems())
        {
            var itemUI = Instantiate(shopItemUIPrefab, shopItemParent).GetComponent<ShoppingUIItem>();
            itemUI.Init(item, inventory, BuyButtonPressed);
            shopItems.Add(itemUI);
        }
    }

    private void RefreshShopItems()
    {
        foreach (var itemUI in shopItems)
        {
            itemUI.RefreshItem();
        }
    }

    private void BuyButtonPressed(ShopItemData itemData)
    {
        inventory.TotalCash -= itemData.ItemCost;
        inventory.AddItem(itemData.ItemType, 1);

        RefreshShopItems();

        DataPersistenceManager.instance.SaveGame();
    }

    #endregion
}