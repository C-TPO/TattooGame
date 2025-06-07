using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class TattooClientManager : MonoBehaviour, IDataPersistence
{
    public static TattooClientManager instance {get; private set;}

    [SerializeField] private TattooClient[] tattooClientPrefabs = {};

    private List<TattooClientData> tattooClientData = new List<TattooClientData>();

    #region Unity Messages

    private void Awake()
    {
        instance = this;

        RetrieveClientData();
    }

    #endregion

    #region Public API

    public TattooClient GetClientByName(string name)
    {
        return tattooClientPrefabs.Where(c => c.ClientName == name).FirstOrDefault(null);
    }

    public List<TattooClientData> GetRandomClientsList(int numClients)
    {
        var randomList = tattooClientData.OrderBy(x => Guid.NewGuid()).ToList();
        randomList.RemoveRange(numClients,randomList.Count - numClients);
        return randomList;
    }

    public void UpdateClientData(TattooClientData clientData)
    {
        for(int i=0; i < tattooClientData.Count; i++)
        {
            if(tattooClientData[i].clientName == clientData.clientName)
            {
                tattooClientData[i] = clientData;
                break;
            }
        }

        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        RetrieveClientData();
    }

    public void SaveData(GameData data)
    {
        data.tattooClients = tattooClientData;
    }

    #endregion

    #region Implementation

    private void RetrieveClientData()
    {
        tattooClientData = DataPersistenceManager.instance.GameData.tattooClients;

        if(tattooClientData.Count == 0)
        {
            foreach(TattooClient c in tattooClientPrefabs)
                tattooClientData.Add(new TattooClientData(c.ClientName, c.ToleranceLevel, c.RecoveryLevel));
            
            DataPersistenceManager.instance.SaveGame();
        }
    }

    #endregion
}