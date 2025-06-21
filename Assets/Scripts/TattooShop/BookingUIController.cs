using System;
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
    private TattooClientBookingData bookedClientData = null;

    private const int numClientsToChoose = 3;

    #region Public API

    public Action OnClientBooked;

    public void Show()
    {
        bookedClientData = DataPersistenceManager.instance.GameData.currentBookedClient;
        InitClients();
        container.SetActive(true);
    }

    public void Hide()
    {
        container.SetActive(false);
    }

    public void ClientSelected(BookingUIClient client)
    {
        //Save current client incase app is closed before tattoo is complete
        client.TattooClientBookingData.clientData.isNewClient = false;//TODO: remove? toggle this after the tattoo
        DataPersistenceManager.instance.GameData.currentBookedClient = client.TattooClientBookingData;

        //Clear out stored list
        clientBookingData.Clear();
        foreach (var c in clients)
        {
            Destroy(c.gameObject);
        }
        clients.Clear();
        TattooClientManager.instance.UpdateClientData(client.TattooClientBookingData.clientData);
        Hide();

        OnClientBooked?.Invoke();
    }

    public void StartTattoo()
    {
        SceneLoader.Load(SceneLoader.GameScene.TattooScene);
    }

    public void LoadData(GameData data)
    {
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
        if (clientParent.childCount != 0)
            return;

        if (bookedClientData != null && bookedClientData.clientData.clientName != string.Empty)
        {
            //If we have a booked client, just show them
            var clientUI = Instantiate(clientUIPrefab, clientParent).GetComponent<BookingUIClient>();
            clientUI.Init(bookedClientData, delegate { OnBookedClientClicked(clientUI); });
            clients.Add(clientUI);
            return;
        }

        if (clientBookingData.Count == 0)
        {
            var potentialClients = TattooClientManager.instance.GetRandomClientsList(numClientsToChoose);
            foreach (var c in potentialClients)
                clientBookingData.Add(new TattooClientBookingData(c, TattooStencilManager.instance.GetRandomStencilIndex()));//TODO: find a more random way to set these

            DataPersistenceManager.instance.SaveGame();
        }

        for (int i = 0; i < clientBookingData.Count; i++)
        {
            var clientUI = Instantiate(clientUIPrefab, clientParent).GetComponent<BookingUIClient>();
            clientUI.Init(clientBookingData[i], delegate { OnClientClicked(clientUI); });
            clients.Add(clientUI);
        }
    }

    private void OnClientClicked(BookingUIClient client)
    {
        genericPopupController.Open("Are you sure you want to book " + client.ClientName + "?", () => ClientSelected(client));//TODO: LOCALIZE
    }
    
    private void OnBookedClientClicked(BookingUIClient client)
    {
        genericPopupController.Open("Start tattooing " + client.ClientName + "?", () => StartTattoo());//TODO: LOCALIZE
    }

    #endregion
}