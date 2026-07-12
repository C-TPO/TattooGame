using DG.Tweening;
using UnityEngine;

public class TattooMachineController : MonoBehaviour
{
    [SerializeField] private TattooInputController inputController = null;
    [SerializeField] private Transform machineTransform = null;
    [SerializeField] private RectTransform tipTransform = null;
    [SerializeField] private float tipSizeMultiplier = 1000f;

    private Vector3 machineIdlePosition = Vector3.zero;
    private bool isOn = false;

    #region Unity Messages

    private void Awake()
    {
        machineIdlePosition = machineTransform.localPosition;
    }

    private void LateUpdate()
    {
        if (!inputController.HasPointerWorldPosition)
        {
            return;
        }

        transform.position = inputController.CurrentWorldPosition;
    }

    private void OnDisable()
    {
        TurnOff();
    }

    #endregion

    #region Public API

    public void TurnOn()
    {
        if (isOn)
        {
            return;
        }

        machineTransform.DOShakePosition(
            duration: 0.1f,
            strength: 2f,
            vibrato: 10,
            randomness: 90f,
            snapping: false,
            fadeOut: false
        ).SetLoops(-1, LoopType.Restart);

        isOn = true;
    }

    public void TurnOff()
    {
        if(machineTransform == null)
        {
            return;
        }
        machineTransform.DOKill();
        machineTransform.localPosition = machineIdlePosition;

        isOn = false;
    }

    public void UpdateTipSize(float size)
    {
        float displayedSize = size * tipSizeMultiplier;

        tipTransform.sizeDelta = new Vector2(
            displayedSize,
            displayedSize
        );
    }

    #endregion
}
