using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int tattoosCompleted;
    public int currentTimeElapsed = 0;
    public Inventory inventory;
    public List<TattooClientData> tattooClients;
    public List<TattooClientBookingData> currentBookingList;
    public TattooClientBookingData currentBookedClient;

    public GameData()
    {
        tattoosCompleted = 0;
        currentTimeElapsed = 0;
        inventory = new Inventory();
        tattooClients = new List<TattooClientData>();
        currentBookingList = new List<TattooClientBookingData>();
        currentBookedClient = null;
    }
}

[System.Serializable]
public class InventoryItem
{
    public ShopItemType ItemType;
    public int Quantity;
}

[System.Serializable]
public class Inventory
{
    public event System.Action OnCashChanged;

    [SerializeField]private int totalCash = 0;
    public int TotalCash
    {
        get => totalCash;
        set
        {
            totalCash = value;
            OnCashChanged?.Invoke();
        }
    }

    [SerializeField]private List<InventoryItem> items = new List<InventoryItem>();

    public Inventory()
    {
        foreach (ShopItemType type in System.Enum.GetValues(typeof(ShopItemType)))
        {
            items.Add(new InventoryItem { ItemType = type, Quantity = 0 });
        }
    }

    public int GetQuantity(ShopItemType itemType)
    {
        var item = items.Find(i => i.ItemType == itemType);
        return item != null ? item.Quantity : 0;
    }

    public void AddItem(ShopItemType itemType, int amount)
    {
        var item = items.Find(i => i.ItemType == itemType);
        if (item != null)
            item.Quantity += amount;
        else
            items.Add(new InventoryItem { ItemType = itemType, Quantity = amount });
    }

    public void SetQuantity(ShopItemType itemType, int amount)
    {
        var item = items.Find(i => i.ItemType == itemType);
        if (item != null)
            item.Quantity = amount;
        else
            items.Add(new InventoryItem { ItemType = itemType, Quantity = amount });
    }

    public static readonly Dictionary<ShopItemType, ShopItemData> ItemDefinitions = new Dictionary<ShopItemType, ShopItemData>
    {
        { ShopItemType.Candy, new ShopItemData(ShopItemType.Candy, "Candy", 5, "shop_candy_desc") },
        { ShopItemType.NumbingSpray, new ShopItemData(ShopItemType.NumbingSpray, "Numbing Spray", 10, "shop_spray_desc") },
        { ShopItemType.NumbingCream, new ShopItemData(ShopItemType.NumbingCream, "Numbing Cream", 15, "shop_cream_desc") },
        { ShopItemType.InkPack1, new ShopItemData(ShopItemType.InkPack1, "Ink Pack #1", 100, "shop_inkpack1_desc", true) },
    };

    public List<ShopItemData> GetShopItems()
    {
        var items = new List<ShopItemData>();
        foreach (var kvp in ItemDefinitions)
        {
            var def = kvp.Value;
            var item = new ShopItemData(def.ItemType, def.DisplayName, def.ItemCost, def.DescriptionKey, def.IsOneTimePurchase);
            items.Add(item);
        }
        return items;
    }

    public bool IsSoldOut(ShopItemType itemType)
    {
        var itemDef = ItemDefinitions[itemType];
        if (!itemDef.IsOneTimePurchase)
            return false;

        var item = items.Find(i => i.ItemType == itemType);
        return item == null || item.Quantity <= 0;
    }
}

public enum ShopItemType { Candy, NumbingSpray, NumbingCream, InkPack1 }

[System.Serializable]
public class ShopItemData
{
    public ShopItemType ItemType;
    public string DisplayName;
    public bool IsOneTimePurchase;
    public string DescriptionKey;
    public int ItemCost;

    public ShopItemData(ShopItemType type, string name, int cost, string descKey = "", bool oneTimePurchase = false)
    {
        ItemType = type;
        DisplayName = name;
        ItemCost = cost;
        DescriptionKey = descKey;
        IsOneTimePurchase = oneTimePurchase;
    }
}