using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private Camera tattooCamera = null;
    [SerializeField] private LayerMask stencilMask;
    [SerializeField] private LayerMask tattooMask;

    #region Public API

    public float ScoreTattoo(SpriteRenderer stencil, out Texture2D tattooTexture)
    {
        tattooCamera.cullingMask = tattooMask;

        int texWidth = (int)(stencil.bounds.size.x * stencil.sprite.pixelsPerUnit);
        int texHeight = (int)(stencil.bounds.size.y * stencil.sprite.pixelsPerUnit);

        RenderTexture tattooRT = new RenderTexture(texWidth,texHeight,16);
        tattooCamera.targetTexture = tattooRT;
        RenderTexture.active = tattooRT;
        tattooCamera.Render();

        tattooTexture = new Texture2D(texWidth,texHeight);
        tattooTexture.ReadPixels(new Rect(0,0,texWidth,texHeight),0,0);
        tattooTexture.Apply();
        RenderTexture.active = null;

        tattooCamera.cullingMask = stencilMask;

        stencil.color = Color.white;

        RenderTexture stencilRT = new RenderTexture(texWidth,texHeight,16);
        tattooCamera.targetTexture = stencilRT;
        RenderTexture.active = stencilRT;
        tattooCamera.Render();

        Texture2D stencilTexture = new Texture2D(texWidth,texHeight);
        stencilTexture.ReadPixels(new Rect(0,0,texWidth,texHeight),0,0);
        RenderTexture.active = null;

        float score = CompareTextures(tattooTexture.GetPixels(), stencilTexture.GetPixels());

        Destroy(stencilTexture);

        return score;
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