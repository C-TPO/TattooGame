using UnityEngine;
using UnityEngine.UI;

public class WorkTimer : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Image timerImage = null;

    private const float hourIncrement = .125f;

    #region Public API

    public void ResetTimer()
    {
        timerImage.fillAmount = 0f;
    }

    public void LoadData(GameData data)
    {
        IncreaseTimer(data.currentTimeElapsed);
    }

    public void SaveData(GameData data)
    {

    }

    #endregion

    #region Implementation

    private bool IncreaseTimer(int numHours)
    {
        float amountToIncrease = hourIncrement * numHours;
        if (timerImage.fillAmount + amountToIncrease > 1.0f)
            return false;

        timerImage.fillAmount += hourIncrement * numHours;
        
        return true;
    }

    #endregion
}
