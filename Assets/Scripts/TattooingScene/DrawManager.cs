using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [SerializeField] private Line linePrefab = null;
    [SerializeField] private Transform lineParent = null;
    [SerializeField] private PainToleranceMeter painMeter = null;

    private Camera mainCamera = null;
    private Line currentLine = null;
    private Vector2 currentMousePos = Vector2.zero;
    private Bounds currentBounds;
    private TattooClientData currentClient = null;
    private bool isTattooing = false;
    private bool canTattoo = false;
    private float lineWidth = .2f;

    #region Unity Messages

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!canTattoo)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            currentMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (!IsInBounds(currentMousePos))
                return;
            isTattooing = true;
            currentLine = Instantiate(linePrefab, currentMousePos, Quaternion.identity, lineParent);
            currentLine.SetLineWidth(lineWidth);
            currentLine.SetPosition(currentMousePos);

            //TODO: Start sfx
        }

        if (isTattooing && Input.GetMouseButton(0))
        {
            currentMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (!IsInBounds(currentMousePos))
                return;

            currentLine.SetPosition(currentMousePos);
            painMeter.UpdateMeter(GetToleranceFromEnum(currentClient.painSensitivity));
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentLine = null;
            isTattooing = false;

            //TODO: End sfx
        }

        if (!isTattooing)
        {
            painMeter.UpdateMeter(GetRecoveryFromEnum(currentClient.painRecoveryRate));
        }
    }

    #endregion

    #region Public API

    public void EnableTattooing(TattooClientBookingData client, SpriteRenderer stencil)
    {
        currentClient = client.clientData;
        currentBounds = stencil.localBounds;
        canTattoo = true;
        print(currentClient.clientName + "  " + currentClient.painRecoveryRate + "  " + currentClient.painSensitivity);
    }

    public void UpdateLineWidth(float value)
    {
        //Wired up in inspector
        lineWidth = value;
    }

    #endregion

    #region Implementation

    private bool IsInBounds(Vector3 mousePos)
    {
        return currentBounds.Contains(mousePos);
    }

    private float GetToleranceFromEnum(TattooClientData.ClientTolerances tolerance)
    {
        //TODO: maybe randomize value and tolerance is a modifier??
        switch (tolerance)
        {
            case TattooClientData.ClientTolerances.Low:
                return 2f;
            case TattooClientData.ClientTolerances.Medium:
                return 1f;
            case TattooClientData.ClientTolerances.High:
                return .5f;
            default:
                return .5f;
        }
    }

    private float GetRecoveryFromEnum(TattooClientData.ClientTolerances recovery)
    {
        //TODO: maybe randomize value and recovery is a modifier??
        switch (recovery)
        {
            case TattooClientData.ClientTolerances.Low:
                return -1.0f;
            case TattooClientData.ClientTolerances.Medium:
                return -1.8f;
            case TattooClientData.ClientTolerances.High:
                return -2.4f;
            default:
                return -1f;
        }
    }

    #endregion
}
