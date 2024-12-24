using UnityEngine;
using UnityEngine.UI;

public class WorkTimer : MonoBehaviour
{
    [SerializeField] private Image timerImage = null;

    private const float hourIncrement = 1.25f;

    public bool IncreaseTimer(int numHours)
    {
        float amountToIncrease = hourIncrement * numHours;

        if(timerImage.fillAmount + amountToIncrease > 1.0f)
            return false;

        timerImage.fillAmount += hourIncrement * numHours;
        
        return true;
    }

    public void ResetTimer()
    {
        timerImage.fillAmount = 0f;
    }
}
