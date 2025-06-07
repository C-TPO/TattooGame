using UnityEngine;

public class TattooClient : MonoBehaviour
{
    [SerializeField] private string clientName = "";
    [SerializeField] private TattooClientData.ClientTolerances toleranceLevel = TattooClientData.ClientTolerances.Low;
    [SerializeField] private TattooClientData.ClientTolerances recoveryLevel = TattooClientData.ClientTolerances.Low;

    private TattooClientData clientData = null;

    #region Unity Messages

    #endregion

    #region Public API

    public TattooClientData ClientData => clientData;
    public string ClientName => clientName;
    public TattooClientData.ClientTolerances ToleranceLevel => toleranceLevel;
    public TattooClientData.ClientTolerances RecoveryLevel => recoveryLevel;

    public void Init(TattooClientData clientData)
    {
        this.clientData = clientData;
    }

    #endregion

    #region Implementation

    #endregion
}
