using System;
using UnityEngine;

public class TattooInputController : MonoBehaviour
{
    [SerializeField] private Camera inputCamera = null;

    private SpriteRenderer drawingSurface = null;
    private bool isInitialized = false;
    private bool inputEnabled = false;
    private bool strokeActive = false;
    private Vector2 lastValidUv = Vector2.zero;

    public event Action<Vector2> OnStrokeStarted;
    public event Action<Vector2> OnStrokeMoved;
    public event Action<Vector2> OnStrokeEnded;

    public Vector3 CurrentWorldPosition { get; private set; }
    public bool HasPointerWorldPosition { get; private set; }

    #region Unity Messages

    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        UpdatePointerWorldPosition(Input.mousePosition);

        if (!inputEnabled)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryStartStroke(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && strokeActive)
        {
            UpdateStroke(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && strokeActive)
        {
            EndStroke();
        }
    }

    private void OnDisable()
    {
        if (strokeActive)
        {
            EndStroke();
        }
    }

    #endregion

    #region Public API

    public void Initialize(SpriteRenderer surface)
    {
        drawingSurface = surface;
        isInitialized = true;

        UpdatePointerWorldPosition(Input.mousePosition);
    }

    public void EnableInput()
    {
        inputEnabled = true;
    }

    public void DisableInput()
    {
        inputEnabled = false;

        if (strokeActive)
        {
            EndStroke();
        }
    }

    #endregion

    #region Implementation

    private void TryStartStroke(Vector2 screenPosition)
    {
        if (!TryGetSurfacePosition(screenPosition, out Vector3 worldPosition, out Vector2 uv))
        {
            return;
        }

        CurrentWorldPosition = worldPosition;
        HasPointerWorldPosition = true;
        lastValidUv = uv;
        strokeActive = true;

        OnStrokeStarted?.Invoke(uv);
    }

    private void UpdateStroke(Vector2 screenPosition)
    {
        if (!TryGetSurfacePosition(screenPosition, out Vector3 worldPosition, out Vector2 uv))
        {
            EndStroke();
            return;
        }

        CurrentWorldPosition = worldPosition;
        HasPointerWorldPosition = true;
        lastValidUv = uv;

        OnStrokeMoved?.Invoke(uv);
    }

    private void EndStroke()
    {
        strokeActive = false;
        OnStrokeEnded?.Invoke(lastValidUv);
    }

    private void UpdatePointerWorldPosition(Vector2 screenPosition)
    {
        Ray ray = inputCamera.ScreenPointToRay(screenPosition);
        Plane surfacePlane = new Plane(
            drawingSurface.transform.forward,
            drawingSurface.transform.position
        );

        if (!surfacePlane.Raycast(ray, out float distance))
        {
            HasPointerWorldPosition = false;
            return;
        }

        CurrentWorldPosition = ray.GetPoint(distance);
        HasPointerWorldPosition = true;
    }

    private bool TryGetSurfacePosition(Vector2 screenPosition, out Vector3 worldPosition, out Vector2 uv)
    {
        Ray ray = inputCamera.ScreenPointToRay(screenPosition);
        Plane surfacePlane = new Plane(
            drawingSurface.transform.forward,
            drawingSurface.transform.position
        );

        if (!surfacePlane.Raycast(ray, out float distance))
        {
            worldPosition = Vector3.zero;
            uv = Vector2.zero;
            return false;
        }

        worldPosition = ray.GetPoint(distance);

        Vector3 localPosition = drawingSurface.transform.InverseTransformPoint(
            worldPosition
        );

        Bounds spriteBounds = drawingSurface.sprite.bounds;

        if (localPosition.x < spriteBounds.min.x
            || localPosition.x > spriteBounds.max.x
            || localPosition.y < spriteBounds.min.y
            || localPosition.y > spriteBounds.max.y)
        {
            uv = Vector2.zero;
            return false;
        }

        uv = new Vector2(
            Mathf.InverseLerp(
                spriteBounds.min.x,
                spriteBounds.max.x,
                localPosition.x
            ),
            Mathf.InverseLerp(
                spriteBounds.min.y,
                spriteBounds.max.y,
                localPosition.y
            )
        );

        return true;
    }

    #endregion
}
