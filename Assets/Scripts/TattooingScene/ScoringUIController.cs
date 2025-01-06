using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoringUIController : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private Image tattooImage;
    [SerializeField] private Star[] stars;
    [SerializeField] private Color highlightColor = Color.white;
    [SerializeField] private Color goldColor = Color.yellow;

    public void Show(int numStars, Texture2D tattooTex)
    {
        continueButton.SetActive(false);

        Sprite tattooSpr = Sprite.Create(tattooTex, new Rect(0f,0f,tattooTex.width, tattooTex.height), new Vector2(.5f, .5f), 100f);
        tattooImage.sprite = tattooSpr;
        tattooImage.SetNativeSize();
        
        gameObject.SetActive(true);

        StartCoroutine(FillStars(numStars));
    }

    private IEnumerator FillStars(int numStars)
    {
        int count = 0;

        foreach(Star star in stars)
        {
            if(count < numStars)
            {
                count++;
                star.ShowStar(highlightColor);
                yield return new WaitForSeconds(.5f);
            }
            else
            {
                break;
            }
        }

        if(numStars == 4)
        {
            foreach(Star star in stars)
                star.ShowStar(goldColor);
        }

        yield return new WaitForSeconds(1.5f);

        continueButton.SetActive(true);
    }
}
