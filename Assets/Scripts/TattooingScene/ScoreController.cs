using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Header("Mask")]
    [SerializeField, Range(0f, 1f)] private float targetAlphaThreshold = 0.1f;
    [SerializeField, Range(0f, 1f)] private float tattooAlphaThreshold = 0.1f;

    [Header("Tolerance")]
    [SerializeField, Range(0.001f, 0.1f)]
    private float coverageToleranceNormalized = 0.04f;

    [SerializeField, Range(0.001f, 0.1f)]
    private float precisionToleranceNormalized = 0.025f;

    [SerializeField, Range(0f, 1f)]
    private float fullCreditRatio = 0.5f;

    [Header("Weights")]
    [SerializeField, Range(0f, 1f)] private float coverageWeight = 0.6f;
    [SerializeField, Range(0f, 1f)] private float precisionWeight = 0.4f;

    [Header("Score Adjustment")]
    [SerializeField, Range(0.25f, 1f)]
    private float scoreCurvePower = 0.75f;

    private const float DiagonalDistance = 1.41421356f;
    private const float InfiniteDistance = 1000000f;

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

        bool[] targetMask = CreateMask(
            targetTexture.GetPixels32(),
            targetAlphaThreshold
        );

        bool[] tattooMask = CreateMask(
            tattooTexture.GetPixels32(),
            tattooAlphaThreshold
        );

        float[] distanceToTarget = CreateDistanceMap(
            targetMask,
            tattooTexture.width,
            tattooTexture.height
        );

        float[] distanceToTattoo = CreateDistanceMap(
            tattooMask,
            tattooTexture.width,
            tattooTexture.height
        );

        float coverageTolerancePixels =
            tattooTexture.width
            * coverageToleranceNormalized;

        float precisionTolerancePixels =
            tattooTexture.width
            * precisionToleranceNormalized;

        float coverage = CalculateMatchScore(
            targetMask,
            distanceToTattoo,
            coverageTolerancePixels
        );

        float precision = CalculateMatchScore(
            tattooMask,
            distanceToTarget,
            precisionTolerancePixels
        );

        float totalWeight =
            coverageWeight
            + precisionWeight;

        float rawScore = 0f;

        if (totalWeight > 0f)
        {
            rawScore = (
                coverage * coverageWeight
                + precision * precisionWeight
            ) / totalWeight;
        }

        float adjustedScore = Mathf.Pow(
            Mathf.Clamp01(rawScore),
            scoreCurvePower
        );

        TattooScoreResult result =
            new TattooScoreResult(
                adjustedScore * 100f,
                coverage * 100f,
                precision * 100f
            );

        Debug.Log(
            $"Tattoo Score: {result.totalScore:0.0} | "
            + $"Raw: {rawScore * 100f:0.0} | "
            + $"Coverage: {result.lineCoverage:0.0} | "
            + $"Precision: {result.linePrecision:0.0}"
        );

        return result;
    }

    #endregion

    #region Texture Conversion

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

        RenderTexture targetRenderTexture = RenderTexture.GetTemporary(
            width,
            height,
            0,
            RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Default
        );

        targetRenderTexture.filterMode = FilterMode.Bilinear;

        Graphics.Blit(
            sourceTexture,
            targetRenderTexture,
            scale,
            offset
        );

        RenderTexture previousRenderTexture = RenderTexture.active;
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
        RenderTexture.ReleaseTemporary(targetRenderTexture);

        return targetTexture;
    }

    private bool[] CreateMask(Color32[] pixels, float alphaThreshold)
    {
        bool[] mask = new bool[pixels.Length];
        byte threshold = (byte)Mathf.RoundToInt(alphaThreshold * 255f);

        for (int i = 0; i < pixels.Length; i++)
        {
            mask[i] = pixels[i].a >= threshold;
        }

        return mask;
    }

    #endregion

    #region Distance Scoring

    private float[] CreateDistanceMap(bool[] sourceMask, int width, int height)
    {
        float[] distances = new float[sourceMask.Length];

        for (int i = 0; i < sourceMask.Length; i++)
        {
            distances[i] = sourceMask[i]
                ? 0f
                : InfiniteDistance;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                float distance = distances[index];

                if (x > 0)
                {
                    distance = Mathf.Min(
                        distance,
                        distances[index - 1] + 1f
                    );
                }

                if (y > 0)
                {
                    distance = Mathf.Min(
                        distance,
                        distances[index - width] + 1f
                    );

                    if (x > 0)
                    {
                        distance = Mathf.Min(
                            distance,
                            distances[index - width - 1]
                            + DiagonalDistance
                        );
                    }

                    if (x < width - 1)
                    {
                        distance = Mathf.Min(
                            distance,
                            distances[index - width + 1]
                            + DiagonalDistance
                        );
                    }
                }

                distances[index] = distance;
            }
        }

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = width - 1; x >= 0; x--)
            {
                int index = y * width + x;
                float distance = distances[index];

                if (x < width - 1)
                {
                    distance = Mathf.Min(
                        distance,
                        distances[index + 1] + 1f
                    );
                }

                if (y < height - 1)
                {
                    distance = Mathf.Min(
                        distance,
                        distances[index + width] + 1f
                    );

                    if (x < width - 1)
                    {
                        distance = Mathf.Min(
                            distance,
                            distances[index + width + 1]
                            + DiagonalDistance
                        );
                    }

                    if (x > 0)
                    {
                        distance = Mathf.Min(
                            distance,
                            distances[index + width - 1]
                            + DiagonalDistance
                        );
                    }
                }

                distances[index] = distance;
            }
        }

        return distances;
    }

    private float CalculateMatchScore(bool[] pixelsToScore, float[] comparisonDistanceMap, float tolerancePixels)
    {
        float score = 0f;
        int scoredPixelCount = 0;

        float fullCreditDistance =
            tolerancePixels * fullCreditRatio;

        for (int i = 0; i < pixelsToScore.Length; i++)
        {
            if (!pixelsToScore[i])
            {
                continue;
            }

            float distance = comparisonDistanceMap[i];
            float pixelScore;

            if (distance <= fullCreditDistance)
            {
                pixelScore = 1f;
            }
            else
            {
                pixelScore = 1f - Mathf.InverseLerp(
                    fullCreditDistance,
                    tolerancePixels,
                    distance
                );
            }

            score += Mathf.Clamp01(pixelScore);
            scoredPixelCount++;
        }

        if (scoredPixelCount == 0)
        {
            return 0f;
        }

        return score / scoredPixelCount;
    }

    #endregion
}
