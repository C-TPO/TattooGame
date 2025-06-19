using DG.Tweening;

using UnityEngine;

public class TattooMachineController : MonoBehaviour
{
    [SerializeField] private Transform machineTransform = null;
    [SerializeField] private RectTransform tipTransform = null;

    private Vector3 machineIdlePosition = Vector3.one;
    private bool isOn = false;

    #region Unity Messages

    private void Awake()
    {
        machineIdlePosition = machineTransform.localPosition;
    }

    private void LateUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = mousePos;
    }

    #endregion

    #region Public API

    public void TurnOn()
    {
        if(isOn)
            return;
        
        machineTransform.DOShakePosition(
            duration: 0.1f,
            strength: 2.0f,
            vibrato: 10,
            randomness: 90f,
            snapping: false,
            fadeOut: false
        ).SetLoops(-1, LoopType.Restart);

        isOn = true;
    }

    public void TurnOff()
    {
        machineTransform.DOKill();
        machineTransform.localPosition = machineIdlePosition;

        isOn = false;
    }

    public void UpdateTipSize(float size)
    {
        tipTransform.sizeDelta = new Vector2(size * 100, size * 100);
    }
    
    #endregion
}