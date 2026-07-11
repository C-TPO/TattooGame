using UnityEngine;

public static class TattooScoringTextureBuilder
{
    #region Public API

    public static Texture2D CreateTargetTexture(
        SpriteRenderer stencil,
        SpriteRenderer drawingArea,
        int width,
        int height)
    {
        Texture2D readableStencilTexture =
            CreateReadableSpriteTexture(stencil.sprite);

        Color[] stencilPixels =
            readableStencilTexture.GetPixels();

        Color32[] targetPixels =
            new Color32[width * height];

        Bounds drawingBounds =
            drawingArea.sprite.bounds;

        Bounds stencilBounds =
            stencil.sprite.bounds;

        Matrix4x4 drawingToStencilMatrix =
            stencil.transform.worldToLocalMatrix
            * drawingArea.transform.localToWorldMatrix;

        for (int y = 0; y < height; y++)
        {
            float drawingV =
                (y + 0.5f) / height;

            float drawingLocalY = Mathf.Lerp(
                drawingBounds.min.y,
                drawingBounds.max.y,
                drawingV
            );

            for (int x = 0; x < width; x++)
            {
                float drawingU =
                    (x + 0.5f) / width;

                float drawingLocalX = Mathf.Lerp(
                    drawingBounds.min.x,
                    drawingBounds.max.x,
                    drawingU
                );

                Vector3 drawingLocalPosition =
                    new Vector3(
                        drawingLocalX,
                        drawingLocalY,
                        drawingBounds.center.z
                    );

                Vector3 stencilLocalPosition =
                    drawingToStencilMatrix.MultiplyPoint3x4(
                        drawingLocalPosition
                    );

                if (!IsInsideStencil(
                    stencilLocalPosition,
                    stencilBounds))
                {
                    continue;
                }

                float stencilU = Mathf.InverseLerp(
                    stencilBounds.min.x,
                    stencilBounds.max.x,
                    stencilLocalPosition.x
                );

                float stencilV = Mathf.InverseLerp(
                    stencilBounds.min.y,
                    stencilBounds.max.y,
                    stencilLocalPosition.y
                );

                if (stencil.flipX)
                {
                    stencilU = 1f - stencilU;
                }

                if (stencil.flipY)
                {
                    stencilV = 1f - stencilV;
                }

                Color stencilColor = SampleBilinear(
                    stencilPixels,
                    readableStencilTexture.width,
                    readableStencilTexture.height,
                    stencilU,
                    stencilV
                );

                targetPixels[y * width + x] =
                    stencilColor;
            }
        }

        Texture2D targetTexture = new Texture2D(
            width,
            height,
            TextureFormat.RGBA32,
            false
        );

        targetTexture.SetPixels32(targetPixels);
        targetTexture.Apply();

        Object.Destroy(readableStencilTexture);

        return targetTexture;
    }

    #endregion

    #region Implementation

    private static bool IsInsideStencil(
        Vector3 position,
        Bounds bounds)
    {
        return position.x >= bounds.min.x
            && position.x <= bounds.max.x
            && position.y >= bounds.min.y
            && position.y <= bounds.max.y;
    }

    private static Texture2D CreateReadableSpriteTexture(
        Sprite sprite)
    {
        Rect textureRect = sprite.textureRect;
        Texture2D sourceTexture = sprite.texture;

        int width = Mathf.RoundToInt(
            textureRect.width
        );

        int height = Mathf.RoundToInt(
            textureRect.height
        );

        RenderTexture renderTexture =
            RenderTexture.GetTemporary(
                width,
                height,
                0,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.Default
            );

        Vector2 scale = new Vector2(
            textureRect.width
                / sourceTexture.width,
            textureRect.height
                / sourceTexture.height
        );

        Vector2 offset = new Vector2(
            textureRect.x
                / sourceTexture.width,
            textureRect.y
                / sourceTexture.height
        );

        Graphics.Blit(
            sourceTexture,
            renderTexture,
            scale,
            offset
        );

        RenderTexture previousRenderTexture =
            RenderTexture.active;

        RenderTexture.active = renderTexture;

        Texture2D readableTexture = new Texture2D(
            width,
            height,
            TextureFormat.RGBA32,
            false
        );

        readableTexture.ReadPixels(
            new Rect(
                0f,
                0f,
                width,
                height
            ),
            0,
            0
        );

        readableTexture.Apply();

        RenderTexture.active =
            previousRenderTexture;

        RenderTexture.ReleaseTemporary(
            renderTexture
        );

        return readableTexture;
    }

    private static Color SampleBilinear(
        Color[] pixels,
        int width,
        int height,
        float u,
        float v)
    {
        float pixelX =
            Mathf.Clamp01(u) * (width - 1);

        float pixelY =
            Mathf.Clamp01(v) * (height - 1);

        int x0 = Mathf.FloorToInt(pixelX);
        int y0 = Mathf.FloorToInt(pixelY);

        int x1 = Mathf.Min(
            x0 + 1,
            width - 1
        );

        int y1 = Mathf.Min(
            y0 + 1,
            height - 1
        );

        float xLerp = pixelX - x0;
        float yLerp = pixelY - y0;

        Color bottomLeft =
            pixels[y0 * width + x0];

        Color bottomRight =
            pixels[y0 * width + x1];

        Color topLeft =
            pixels[y1 * width + x0];

        Color topRight =
            pixels[y1 * width + x1];

        Color bottom = Color.Lerp(
            bottomLeft,
            bottomRight,
            xLerp
        );

        Color top = Color.Lerp(
            topLeft,
            topRight,
            xLerp
        );

        return Color.Lerp(
            bottom,
            top,
            yLerp
        );
    }

    #endregion
}