using UnityEngine;

public class ScoreControllerLegacy : MonoBehaviour
{
    [Header("Pixel Detection")]
    [SerializeField, Range(0f, 1f)]
    private float targetAlphaThreshold = 0.1f;

    [SerializeField, Range(0f, 1f)]
    private float tattooAlphaThreshold = 0.1f;

    [Header("Score Weights")]
    [SerializeField, Range(0f, 1f)]
    private float targetAccuracyWeight = 0.5f;

    [SerializeField, Range(0f, 1f)]
    private float backgroundAccuracyWeight = 0.5f;

    [Tooltip(
        "The original scorer subtracted the percentage of all incorrect "
        + "pixels after calculating target and background accuracy. "
        + "Set this to 1 to reproduce that behaviour, or 0 to disable it."
    )]
    [SerializeField, Range(0f, 1f)]
    private float overallErrorPenaltyWeight = 1f;

    [Header("Debug")]
    [SerializeField] private bool logScoreBreakdown = true;

    #region Public API

    public TattooScoreResult ScoreTattoo(Texture2D tattooTexture,Texture2D targetTexture)
    {
        if (tattooTexture.width != targetTexture.width
            || tattooTexture.height != targetTexture.height)
        {
            Debug.LogError(
                "Tattoo and target textures must "
                + "have matching dimensions."
            );

            return default;
        }

        return CompareTextures(
            tattooTexture.GetPixels32(),
            targetTexture.GetPixels32()
        );
    }

    #endregion

    #region Implementation

    private TattooScoreResult CompareTextures(Color32[] tattooPixels, Color32[] targetPixels)
    {
        int targetPixelCount = 0;
        int backgroundPixelCount = 0;

        int missedTargetPixelCount = 0;
        int outsideTattooPixelCount = 0;

        byte targetThreshold = GetAlphaThresholdByte(
            targetAlphaThreshold
        );

        byte tattooThreshold = GetAlphaThresholdByte(
            tattooAlphaThreshold
        );

        for (int i = 0; i < targetPixels.Length; i++)
        {
            bool isTargetPixel =
                targetPixels[i].a >= targetThreshold;

            bool isTattooPixel =
                tattooPixels[i].a >= tattooThreshold;

            if (isTargetPixel)
            {
                targetPixelCount++;

                if (!isTattooPixel)
                {
                    missedTargetPixelCount++;
                }
            }
            else
            {
                backgroundPixelCount++;

                if (isTattooPixel)
                {
                    outsideTattooPixelCount++;
                }
            }
        }

        float targetAccuracy = CalculateAccuracy(
            targetPixelCount,
            missedTargetPixelCount
        );

        float backgroundAccuracy = CalculateAccuracy(
            backgroundPixelCount,
            outsideTattooPixelCount
        );

        float totalWeight =
            targetAccuracyWeight
            + backgroundAccuracyWeight;

        float weightedScore = 0f;

        if (totalWeight > 0f)
        {
            weightedScore = (
                targetAccuracy * targetAccuracyWeight
                + backgroundAccuracy * backgroundAccuracyWeight
            ) / totalWeight;
        }

        int totalPixelCount = targetPixels.Length;

        float overallErrorRate = totalPixelCount > 0
            ? (float)(
                missedTargetPixelCount
                + outsideTattooPixelCount
            ) / totalPixelCount
            : 0f;

        float finalScore = weightedScore
            - overallErrorRate * overallErrorPenaltyWeight;

        finalScore = Mathf.Clamp01(finalScore);

        TattooScoreResult result = new TattooScoreResult(
            finalScore * 100f,
            targetAccuracy * 100f,
            backgroundAccuracy * 100f
        );

        if (logScoreBreakdown)
        {
            Debug.Log(
                $"Tattoo Score (LEGACY): {result.totalScore:0.0} | "
                + $"Target Accuracy: {targetAccuracy * 100f:0.0} | "
                + $"Background Accuracy: "
                + $"{backgroundAccuracy * 100f:0.0} | "
                + $"Missed Target Pixels: "
                + $"{missedTargetPixelCount} | "
                + $"Outside Ink Pixels: "
                + $"{outsideTattooPixelCount} | "
                + $"Overall Error Penalty: "
                + $"{overallErrorRate * overallErrorPenaltyWeight * 100f:0.0}"
            );
        }

        return result;
    }

    private float CalculateAccuracy(int totalPixelCount, int incorrectPixelCount)
    {
        if (totalPixelCount == 0)
        {
            return 1f;
        }

        return 1f
            - (float)incorrectPixelCount / totalPixelCount;
    }

    private byte GetAlphaThresholdByte(float threshold)
    {
        return (byte)Mathf.RoundToInt(
            threshold * byte.MaxValue
        );
    }

    private Texture2D CreateTargetTexture(Sprite targetSprite, int width, int height)
    {
        Rect textureRect = targetSprite.textureRect;
        Texture2D sourceTexture = targetSprite.texture;

        Vector2 scale = new Vector2(
            textureRect.width / sourceTexture.width,
            textureRect.height / sourceTexture.height
        );

        Vector2 offset = new Vector2(
            textureRect.x / sourceTexture.width,
            textureRect.y / sourceTexture.height
        );

        RenderTexture targetRenderTexture =
            RenderTexture.GetTemporary(
                width,
                height,
                0,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.Default
            );

        targetRenderTexture.filterMode = FilterMode.Bilinear;
        targetRenderTexture.wrapMode = TextureWrapMode.Clamp;

        Graphics.Blit(
            sourceTexture,
            targetRenderTexture,
            scale,
            offset
        );

        RenderTexture previousRenderTexture =
            RenderTexture.active;

        RenderTexture.active = targetRenderTexture;

        Texture2D targetTexture = new Texture2D(
            width,
            height,
            TextureFormat.RGBA32,
            false
        );

        targetTexture.ReadPixels(
            new Rect(0f, 0f, width, height),
            0,
            0
        );

        targetTexture.Apply();

        RenderTexture.active = previousRenderTexture;

        RenderTexture.ReleaseTemporary(
            targetRenderTexture
        );

        return targetTexture;
    }

    #endregion
}