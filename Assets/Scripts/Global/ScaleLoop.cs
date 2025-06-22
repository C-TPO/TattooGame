using DG.Tweening;

using UnityEngine;

public class ScaleLoop : MonoBehaviour
{
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float minScale = .9f;
    [SerializeField] private float duration = 2.0f;
    [SerializeField] private Ease ease = Ease.Linear;

    private Vector3 minScaleVector;
    private Vector3 maxScaleVector;

    private void Awake()
    {
        minScaleVector = maxScaleVector = transform.localScale;
        minScaleVector.x = minScaleVector.y = minScale;
        maxScaleVector.x = maxScaleVector.y = maxScale;

        transform.localScale = minScaleVector;
    }

    private void Start()
    {
        Sequence scaleSequence = DOTween.Sequence();

        scaleSequence.Append(transform.DOScale(maxScaleVector, duration))
            .Append(transform.DOScale(minScaleVector, duration))
            .SetEase(ease)
            .SetLoops(-1)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
            .Play();
    }
}
