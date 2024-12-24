using UnityEngine;

[System.Serializable]
public class TattooClientData
{
    #region Public API

    public string clientName;
    public float painSensitivity;
    public float painRecoveryRate;
    public bool isNewClient;

    public TattooClientData(string _name)
    {
        clientName = _name;
        painSensitivity = Random.Range(.5f, 2.5f);
        painRecoveryRate = Random.Range(-1.5f, -2.5f);
        isNewClient = true;
    }

    #endregion
}

[System.Serializable]
public class TattooClientBookingData
{
    public TattooClientData clientData;
    public int tattooDesignIndex;

    public TattooClientBookingData(TattooClientData data, int tattooIndex)
    {
        clientData = data;
        tattooDesignIndex = tattooIndex;
    }
}