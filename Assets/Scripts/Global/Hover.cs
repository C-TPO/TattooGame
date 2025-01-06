using DG.Tweening;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField] private float hoverAmount = 10f;
    [SerializeField] private float duration = 2.0f;
    [SerializeField] private Ease ease = Ease.Linear;

    private float upperPosY = 0f;
    private float lowerPosY = 0f;

    private void Awake()
    {
        Vector3 localPos = transform.localPosition;
        upperPosY = localPos.y + hoverAmount;
        lowerPosY = localPos.y - hoverAmount;
        localPos.y = lowerPosY;
        transform.localPosition = localPos;
    }

    private void Start()
    {
        Sequence hoverSequence = DOTween.Sequence();

        hoverSequence.Append(transform.DOLocalMoveY(upperPosY, duration))
            .Append(transform.DOLocalMoveY(lowerPosY, duration))
            .SetEase(ease)
            .SetLoops(-1)
            .Play();
    }
}
