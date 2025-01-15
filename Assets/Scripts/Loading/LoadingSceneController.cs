using TMPro;

using UnityEngine;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText = null;

    private string defaultText = "";
    private string appendText = ".";
    private int numDots = 0;
    private int counter = 0;

    private const int maxDots = 2;
    private const int incrementTime = 1000;

    private void Awake()
    {
        defaultText = loadingText.text;
    }

    private void Update()
    {
        counter++;

        if(counter >= incrementTime)
        {
            numDots++;
            if(numDots > maxDots)
                numDots = 0;

            counter = 0;

            string newText = defaultText;

            for(int i=0; i < numDots; i++)
                newText += appendText;

            loadingText.text = newText;
        }
    }
}
