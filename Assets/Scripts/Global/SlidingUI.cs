using DG.Tweening;

using UnityEngine;

public class SlidingUI : MonoBehaviour
{
    [SerializeField] private float hideAmountX = 0f;
    [SerializeField] private float hideAmountY = 0f;
    [SerializeField] private RectTransform container = null;
    [SerializeField] private GameObject[] toggles = {};

    private Vector3 originalPos = Vector3.zero;
    private bool isOpen = true;

    private const float slideDuration = .75f;

    private void Start()
    {
        originalPos = container.localPosition;
        Vector3 finalPos = originalPos;
        finalPos.x -= hideAmountX;
        finalPos.y -= hideAmountY;
        container.localPosition = finalPos;
    }

    public void Toggle()
    {
        isOpen = !isOpen;

        if(isOpen)
            Hide();
        else
            Show();

        if(toggles.Length == 2)
        {
            toggles[0].SetActive(isOpen);
            toggles[1].SetActive(!isOpen);
        }
    }

    public void Show()
    {
        if(isOpen)
            return;
        
        container.DOLocalMove(originalPos, slideDuration).SetEase(Ease.OutBounce);
    }

    public void Hide()
    {
        if(!isOpen)
            return;
        
        Vector3 finalPos = originalPos;
        finalPos.x -= hideAmountX;
        finalPos.y -= hideAmountY;
        container.DOLocalMove(finalPos, slideDuration).SetEase(Ease.OutBounce);
    }

}
