using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer = null;

    private const float lineResolution = .01f;

    #region Public API

    public void SetPosition(Vector2 pos)
    {
        if(!CanAppend(pos))
            return;

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount-1,pos);
    }

    public void SetLineWidth(float newWidth)
    {
        lineRenderer.startWidth = newWidth;
        lineRenderer.endWidth = newWidth;
    }

    #endregion

    #region Implementation

    private bool CanAppend(Vector2 pos)
    {
        if(lineRenderer.positionCount == 0)
            return true;

        return Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), pos) > lineResolution;
    }

    #endregion
}