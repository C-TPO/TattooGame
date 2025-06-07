using UnityEngine;

public class ShoppingUIController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject container = null;
    [SerializeField] private GenericPopupController genericPopupController = null;

    private Inventory inventory = null;

    #region Public API

    public void Show()
    {
        //InitClients();
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
}