using System.Collections.Generic;
using UnityEngine;

public class BookingUIController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject container = null;
    [SerializeField] private RectTransform clientParent = null;
    [SerializeField] private GameObject clientUIPrefab = null;
    [SerializeField] private GenericPopupController genericPopupController = null;

    private List<TattooClientBookingData> clientBookingData = new List<TattooClientBookingData>();
    private List<BookingUIClient> clients = new List<BookingUIClient>();

    private const int numClientsToChoose = 3;

    #region Public API

    public void Show()
    {
        InitClients();
        container.SetActive(true);
    }

    public void Hide()
    {
        container.SetActive(false);
    }

    public void ClientSelected(BookingUIClient client)
    {
        print("CLIENT SELECTED " + client.ClientName);
        clientBookingData.Clear();
        foreach(var c in clients)
        {
            Destroy(c.gameObject);
        }
        clients.Clear();
        client.TattooClientBookingData.clientData.isNewClient = false;//TODO: remove?
        TattooClientManager.instance.UpdateClientData(client.TattooClientBookingData.clientData);
        Hide();
        //TODO: launch tattoo scene?
    }

    public void LoadData(GameData data)
    {
        print("LOADeD BOOKING DATA");
        clientBookingData = data.currentBookingList;
    }

    public void SaveData(GameData data)
    {
        data.currentBookingList = clientBookingData;
    }

    #endregion

    #region Implementation

    private void InitClients()
    {
        if(clientParent.childCount != 0)
            return;

        if(clientBookingData.Count == 0)
        {
            var potentialClients = TattooClientManager.instance.GetRandomClientsList(numClientsToChoose);
            foreach(var c in potentialClients)
                clientBookingData.Add(new TattooClientBookingData(c, Random.Range(1,5)));//TODO: Find a better way to choose stencil index
            
            DataPersistenceManager.instance.SaveGame();
        }

        for(int i = 0; i < clientBookingData.Count; i++)
        { 
            var clientUI = Instantiate(clientUIPrefab, clientParent).GetComponent<BookingUIClient>();
            clientUI.Init(clientBookingData[i], delegate{OnClientClicked(clientUI);});
            clients.Add(clientUI);
        }
    }

    private void ClearUI()
    {

    }

    private void OnClientClicked(BookingUIClient client)
    {
        genericPopupController.Open("Are you sure you want to book " + client.ClientName + "?", () => ClientSelected(client));//TODO: LOCALIZE
    }

    #endregion
}