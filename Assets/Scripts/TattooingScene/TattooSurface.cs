using System.Collections.Generic;
using UnityEngine;

public class TattooSurface : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private MeshRenderer displayRenderer = null;
    [SerializeField] private Material displayMaterial = null;

    [Header("Drawing")]
    [SerializeField] private Material brushMaterial = null;
    [SerializeField, Min(64)] private int longEdgeResolution = 1024;

    private static readonly int MainTextureId = Shader.PropertyToID("_MainTex");
    private static readonly int HardnessId = Shader.PropertyToID("_Hardness");

    private RenderTexture canvasTexture = null;
    private Material runtimeDisplayMaterial = null;
    private Material runtimeBrushMaterial = null;
    private SpriteRenderer drawingSurface = null;

    public RenderTexture CanvasTexture => canvasTexture;
    public int Width => canvasTexture.width;
    public int Height => canvasTexture.height;

    #region Public API

    public void Initialize(SpriteRenderer surface)
    {
        drawingSurface = surface;

        CreateRuntimeMaterials();
        AlignDisplayToSurface();

        displayRenderer.sortingLayerID = drawingSurface.sortingLayerID;
        displayRenderer.sortingOrder = drawingSurface.sortingOrder + 1;

        CreateCanvasTexture();
        Clear();

        runtimeDisplayMaterial.SetTexture(MainTextureId, canvasTexture);
        displayRenderer.enabled = true;
    }

    public float GetBrushDiameterPixels(float worldSize)
    {
        float surfaceWorldWidth =
            drawingSurface.sprite.bounds.size.x
            * Mathf.Abs(drawingSurface.transform.lossyScale.x);

        return worldSize / surfaceWorldWidth * Width;
    }

    public void ApplySamples(
        IReadOnlyList<TattooBrushSample> samples,
        TattooBrushDefinition brushDefinition)
    {
        if (samples.Count == 0)
        {
            return;
        }

        RenderTexture previousRenderTexture = RenderTexture.active;
        RenderTexture.active = canvasTexture;

        GL.PushMatrix();
        GL.LoadPixelMatrix(0f, Width, 0f, Height);

        runtimeBrushMaterial.SetFloat(HardnessId, brushDefinition.Hardness);
        runtimeBrushMaterial.SetPass(0);

        GL.Begin(GL.TRIANGLES);

        for (int i = 0; i < samples.Count; i++)
        {
            DrawSample(samples[i]);
        }

        GL.End();
        GL.PopMatrix();

        RenderTexture.active = previousRenderTexture;
    }

    public void Clear()
    {
        RenderTexture previousRenderTexture = RenderTexture.active;
        RenderTexture.active = canvasTexture;

        GL.Clear(false, true, Color.clear);

        RenderTexture.active = previousRenderTexture;
    }

    public Vector2 UvToPixelPosition(Vector2 uv)
    {
        return new Vector2(
            uv.x * Width,
            uv.y * Height
        );
    }

    public Vector2 PixelToUvPosition(Vector2 pixelPosition)
    {
        return new Vector2(
            pixelPosition.x / Width,
            pixelPosition.y / Height
        );
    }

    public Texture2D CreateTexture2D()
    {
        RenderTexture previousRenderTexture = RenderTexture.active;
        RenderTexture.active = canvasTexture;

        Texture2D texture = new Texture2D(
            Width,
            Height,
            TextureFormat.RGBA32,
            false
        );

        texture.ReadPixels(
            new Rect(0f, 0f, Width, Height),
            0,
            0
        );

        texture.Apply();

        RenderTexture.active = previousRenderTexture;

        return texture;
    }

    #endregion

    #region Unity Messages

    private void OnDestroy()
    {
        ReleaseCanvasTexture();

        if (runtimeDisplayMaterial != null)
        {
            Destroy(runtimeDisplayMaterial);
        }

        if (runtimeBrushMaterial != null)
        {
            Destroy(runtimeBrushMaterial);
        }
    }

    #endregion

    #region Implementation

    private void CreateRuntimeMaterials()
    {
        if (runtimeDisplayMaterial == null)
        {
            runtimeDisplayMaterial = new Material(displayMaterial);
            displayRenderer.material = runtimeDisplayMaterial;
        }

        if (runtimeBrushMaterial == null)
        {
            runtimeBrushMaterial = new Material(brushMaterial);
        }
    }

    private void AlignDisplayToSurface()
    {
        Bounds spriteBounds = drawingSurface.sprite.bounds;
        Transform displayTransform = displayRenderer.transform;

        displayTransform.SetParent(drawingSurface.transform, false);
        displayTransform.localPosition = spriteBounds.center;
        displayTransform.localRotation = Quaternion.identity;
        displayTransform.localScale = new Vector3(
            spriteBounds.size.x,
            spriteBounds.size.y,
            1f
        );
    }

    private void CreateCanvasTexture()
    {
        ReleaseCanvasTexture();

        Vector2 surfaceSize = drawingSurface.sprite.bounds.size;

        int width;
        int height;

        if (surfaceSize.x >= surfaceSize.y)
        {
            width = longEdgeResolution;
            height = Mathf.Max(
                64,
                Mathf.RoundToInt(longEdgeResolution * surfaceSize.y / surfaceSize.x)
            );
        }
        else
        {
            height = longEdgeResolution;
            width = Mathf.Max(
                64,
                Mathf.RoundToInt(longEdgeResolution * surfaceSize.x / surfaceSize.y)
            );
        }

        canvasTexture = new RenderTexture(
            width,
            height,
            0,
            RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Default
        )
        {
            name = "Tattoo Canvas",
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp,
            useMipMap = false,
            autoGenerateMips = false
        };

        canvasTexture.Create();
    }

    private void ReleaseCanvasTexture()
    {
        if (canvasTexture == null)
        {
            return;
        }

        canvasTexture.Release();
        Destroy(canvasTexture);
        canvasTexture = null;
    }

    private void DrawSample(TattooBrushSample sample)
    {
        Vector2 center = UvToPixelPosition(sample.uv);
        float diameter = GetBrushDiameterPixels(sample.size);
        float halfSize = diameter * 0.5f;

        float left = center.x - halfSize;
        float right = center.x + halfSize;
        float bottom = center.y - halfSize;
        float top = center.y + halfSize;

        Color vertexColor = sample.color;
        vertexColor.a *= sample.opacity;

        GL.Color(vertexColor);

        AddVertex(left, bottom, 0f, 0f);
        AddVertex(left, top, 0f, 1f);
        AddVertex(right, top, 1f, 1f);

        AddVertex(left, bottom, 0f, 0f);
        AddVertex(right, top, 1f, 1f);
        AddVertex(right, bottom, 1f, 0f);
    }

    private void AddVertex(float x, float y, float u, float v)
    {
        GL.TexCoord2(u, v);
        GL.Vertex3(x, y, 0f);
    }

    #endregion
}
