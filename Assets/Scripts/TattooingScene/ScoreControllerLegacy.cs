using UnityEngine;

public class ScoreControllerLegacy : MonoBehaviour
{
    [SerializeField] private Camera tattooCamera = null;
    [SerializeField] private LayerMask stencilMask;
    [SerializeField] private LayerMask tattooMask;

    #region Public API

    public TattooScoreResult ScoreTattoo(Texture2D tattooTexture, Texture2D targetTexture)
    {
        float score = CompareTextures(
            tattooTexture.GetPixels(),
            targetTexture.GetPixels()
        );

        return new TattooScoreResult
        {
            totalScore = score
        };
    }

    #endregion

    #region Implementation

    private float CompareTextures(Color[] tattooPixels, Color[] stencilPixels)
    {
        float finalScore = 0f;
        float totalPixels = tattooPixels.Length;

        float coloredStencilPixels = 0f;
        float blankStencilPixels = 0f;

        float wrongColoredPixels = 0f;
        float wrongBlankPixels = 0f;

        for(int i = 0; i < totalPixels; i++)
        {
            if(stencilPixels[i].a != 0)//Check colored part of stencil
            {
                coloredStencilPixels++;
                
                if(tattooPixels[i].a != 0)//TODO: Check if correct color once colors are implemented... maybe half points for incorrect color? 
                    continue;

                wrongColoredPixels++;
            }
            else if(stencilPixels[i].a == 0)//Check blank part of stencil
            {
                blankStencilPixels++;

                if(tattooPixels[i].a == 0)
                    continue;

                wrongBlankPixels++;
            }
        }

        var finalScore1 = 100f - 100 * (wrongColoredPixels / coloredStencilPixels);

        var finalScore2 = 100f - 100 * (wrongBlankPixels / blankStencilPixels);

        finalScore = (finalScore1 + finalScore2)/2;
        finalScore = finalScore - ((wrongColoredPixels + wrongBlankPixels) / (coloredStencilPixels + blankStencilPixels) * 100);

        print("FINAL: " + finalScore);
        return finalScore;
    }

    #endregion
}