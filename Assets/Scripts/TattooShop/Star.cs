using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    [SerializeField] private Image starImage;
    [SerializeField] private ParticleSystem starBurst;

    public void ShowStar(Color color)
    {
        starImage.color = color;
        var starBurstMain = starBurst.main;
        starBurstMain.startColor = color;
        starBurst.Play();
    }
}