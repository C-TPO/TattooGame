[System.Serializable]
public class TattooClientData
{
    #region Public API

    public enum ClientTolerances
    {
        Low,
        Medium,
        High
    }

    public string clientName;
    public ClientTolerances painSensitivity;
    public ClientTolerances painRecoveryRate;
    public bool isNewClient;

    public TattooClientData(string _name, ClientTolerances tolerance = ClientTolerances.Low, ClientTolerances recovery = ClientTolerances.Low)
    {
        clientName = _name;
        painSensitivity = tolerance;
        painRecoveryRate = recovery;
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