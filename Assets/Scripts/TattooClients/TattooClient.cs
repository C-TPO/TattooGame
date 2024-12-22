using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class TattooClient : MonoBehaviour
{
    [SerializeField, NotNull] private string clientName = "";
    private TattooClientData clientData = null;

    #region Unity Messages

    #endregion

    #region Public API

    public TattooClientData ClientData => clientData;

    public void Init(TattooClientData clientData)
    {
        this.clientData = clientData;
    }

    #endregion

    #region Implementation

    #endregion
}
