using UnityEngine;

public class TattooClient : MonoBehaviour
{
    [SerializeField] private string clientName = "";
    private TattooClientData clientData = null;

    #region Unity Messages

    #endregion

    #region Public API

    public TattooClientData ClientData => clientData;
    public string ClientName => clientName;

    public void Init(TattooClientData clientData)
    {
        this.clientData = clientData;
    }

    #endregion

    #region Implementation

    #endregion
}
