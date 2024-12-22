using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [SerializeField, NotNull] private Line linePrefab = null;
    [SerializeField, NotNull] private Transform lineParent = null;
    [SerializeField, NotNull] private PainToleranceMeter painMeter = null;

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
        if(!canTattoo)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            currentMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if(!IsInBounds(currentMousePos))
                return;
            isTattooing = true;
            currentLine = Instantiate(linePrefab,currentMousePos,Quaternion.identity,lineParent);
            currentLine.SetLineWidth(lineWidth);
            currentLine.SetPosition(currentMousePos);

            //TODO: Start sfx
        }

        if(isTattooing && Input.GetMouseButton(0))
        {
            currentMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if(!IsInBounds(currentMousePos))
                return;

            currentLine.SetPosition(currentMousePos);
            painMeter.UpdateMeter(currentClient.PainSensitivity);
        }

        if(Input.GetMouseButtonUp(0))
        {
            currentLine = null;
            isTattooing = false;

            //TODO: End sfx
        }

        if(!isTattooing)
        {
            painMeter.UpdateMeter(currentClient.PainRecoveryRate);
        }
    }

    #endregion

    #region Public API

    public void EnableTattooing(TattooClient client, SpriteRenderer stencil)
    {
        currentClient = client.ClientData;
        currentBounds = stencil.localBounds;
        canTattoo = true;
        print(currentClient.ClientName + "  " + currentClient.PainRecoveryRate + "  " + currentClient.PainSensitivity);
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

    #endregion
}
