using System.Collections.Generic;
using UnityEngine;

public class TattooStrokeController : MonoBehaviour
{
    [Header("Drawing")]
    [SerializeField] private TattooInputController inputController = null;
    [SerializeField] private TattooSurface tattooCanvas = null;
    [SerializeField] private TattooBrushDefinition lineBrush = null;

    [Header("Feedback")]
    [SerializeField] private TattooMachineController tattooMachineController = null;
    [SerializeField] private PainToleranceMeter painMeter = null;

    private readonly List<TattooBrushSample> pendingSamples = new();

    private bool sessionActive = false;
    private bool isTattooing = false;

    private float brushSize = 0.02f;
    private float brushOpacity = 1f;
    private Color brushColor = Color.black;

    private float painMultiplier = 1f;
    private float recoveryPerSecond = 1f;

    private Vector2 previousPointerPixel = Vector2.zero;
    private Vector2 lastStampPixel = Vector2.zero;
    private float distanceUntilNextStamp = 0f;

    public bool IsTattooing => isTattooing;

    #region Unity Messages

    private void OnEnable()
    {
        inputController.OnStrokeStarted += HandleStrokeStarted;
        inputController.OnStrokeMoved += HandleStrokeMoved;
        inputController.OnStrokeEnded += HandleStrokeEnded;
    }

    private void OnDisable()
    {
        inputController.OnStrokeStarted -= HandleStrokeStarted;
        inputController.OnStrokeMoved -= HandleStrokeMoved;
        inputController.OnStrokeEnded -= HandleStrokeEnded;

        tattooMachineController.TurnOff();
        isTattooing = false;
    }

    private void Update()
    {
        if (!sessionActive)
        {
            return;
        }

        if (isTattooing)
        {
            painMeter.AddPain(
                lineBrush.PainPerSecond
                * painMultiplier
                * Time.deltaTime
            );
        }
        else
        {
            painMeter.Recover(
                recoveryPerSecond
                * Time.deltaTime
            );
        }
    }

    #endregion

    #region Public API

    public void Initialize(float clientPainMultiplier, float clientRecoveryPerSecond)
    {
        painMultiplier = clientPainMultiplier;
        recoveryPerSecond = clientRecoveryPerSecond;

        brushSize = lineBrush.Size;
        brushOpacity = lineBrush.Opacity;
        brushColor = lineBrush.Color;

        tattooMachineController.UpdateTipSize(brushSize);
    }

    public void EnableTattooing()
    {
        sessionActive = true;
        inputController.EnableInput();
    }

    public void DisableTattooing()
    {
        sessionActive = false;
        inputController.DisableInput();

        tattooMachineController.TurnOff();
        isTattooing = false;
    }

    public void UpdateLineWidth(float value)
    {
        brushSize = value;
        tattooMachineController.UpdateTipSize(value);
    }

    public void UpdateOpacity(float value)
    {
        brushOpacity = value;
    }

    public void UpdateColor(Color value)
    {
        brushColor = value;
    }

    public void ClearTattoo()
    {
        tattooCanvas.Clear();
    }

    #endregion

    #region Input Handling

    private void HandleStrokeStarted(Vector2 uv)
    {
        if (!sessionActive)
        {
            return;
        }

        isTattooing = true;

        previousPointerPixel = tattooCanvas.UvToPixelPosition(uv);
        lastStampPixel = previousPointerPixel;
        distanceUntilNextStamp = GetStampSpacingPixels();

        pendingSamples.Clear();
        pendingSamples.Add(CreateSample(
            uv,
            Vector2.zero,
            0f
        ));

        tattooCanvas.ApplySamples(pendingSamples, lineBrush);
        tattooMachineController.TurnOn();
    }

    private void HandleStrokeMoved(Vector2 uv)
    {
        if (!isTattooing)
        {
            return;
        }

        Vector2 currentPointerPixel = tattooCanvas.UvToPixelPosition(uv);
        Vector2 segment = currentPointerPixel - previousPointerPixel;
        float segmentLength = segment.magnitude;

        if (segmentLength <= Mathf.Epsilon)
        {
            return;
        }

        Vector2 direction = segment / segmentLength;
        float speed = segmentLength / Mathf.Max(Time.deltaTime, 0.0001f);
        float spacing = GetStampSpacingPixels();
        float distanceAlongSegment = distanceUntilNextStamp;

        pendingSamples.Clear();

        while (distanceAlongSegment <= segmentLength)
        {
            Vector2 samplePixel = previousPointerPixel
                + direction * distanceAlongSegment;

            Vector2 sampleUv = tattooCanvas.PixelToUvPosition(
                samplePixel
            );

            pendingSamples.Add(CreateSample(
                sampleUv,
                direction,
                speed
            ));

            lastStampPixel = samplePixel;
            distanceAlongSegment += spacing;
        }

        distanceUntilNextStamp = distanceAlongSegment - segmentLength;
        previousPointerPixel = currentPointerPixel;

        tattooCanvas.ApplySamples(pendingSamples, lineBrush);
    }

    private void HandleStrokeEnded(Vector2 uv)
    {
        if (!isTattooing)
        {
            return;
        }

        Vector2 finalPixel = tattooCanvas.UvToPixelPosition(uv);
        Vector2 finalDirection = finalPixel - lastStampPixel;

        if (finalDirection.sqrMagnitude > 0.25f)
        {
            pendingSamples.Clear();
            pendingSamples.Add(CreateSample(
                uv,
                finalDirection.normalized,
                0f
            ));

            tattooCanvas.ApplySamples(pendingSamples, lineBrush);
        }

        tattooMachineController.TurnOff();
        isTattooing = false;
    }

    #endregion

    #region Implementation

    private TattooBrushSample CreateSample(Vector2 uv, Vector2 direction, float speed)
    {
        return new TattooBrushSample(
            uv,
            direction,
            speed,
            brushSize,
            brushOpacity,
            brushColor
        );
    }

    private float GetStampSpacingPixels()
    {
        float brushDiameterPixels = tattooCanvas.GetBrushDiameterPixels(brushSize);

        return Mathf.Max(
            1f,
            brushDiameterPixels * lineBrush.Spacing
        );
    }

    #endregion
}
