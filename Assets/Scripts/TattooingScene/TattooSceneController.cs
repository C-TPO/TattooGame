using System.Collections;

using UnityEngine;

public class TattooSceneController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private SpriteRenderer stencil = null;
    [SerializeField] private DrawManager drawManager = null;
    [SerializeField] private ScoreController scoreController = null;
    [SerializeField] private ScoringUIController scoringUIController = null;
    
    private TattooClientBookingData currentClientData;
    private TattooStencilScriptableObject currentStencilData;
    private bool isTattooComplete = false;

    #region Unity Messages

    void Start()
    {
        currentClientData = DataPersistenceManager.instance.GameData.currentBookedClient;
        currentStencilData = TattooStencilManager.instance.GetStencilByIndex(currentClientData.tattooDesignIndex);
        
        stencil.sprite = currentStencilData.sprite;

        drawManager.EnableTattooing(currentClientData, stencil);

        Cursor.visible = false;
    }

    #endregion

    #region Public API

    public void ValidateTattoo()
    {
        //Wired up in inspector
        float score = scoreController.ScoreTattoo(stencil, out Texture2D tattooTexture);
        isTattooComplete = true;

        StartCoroutine(ShowScore(score, tattooTexture));
    }

    public void LoadData(GameData data)
    {

    }

    public void SaveData(GameData data)
    {
        if(!isTattooComplete)
            return;
        
        data.currentBookedClient = null;
        data.currentTimeElapsed += currentStencilData.duration;
        data.inventory.TotalCash += 100;//TODO: redo this based on difficulty, score, etc.
    }

    public void ContinuePressed()
    {
        SceneLoader.Load(SceneLoader.GameScene.ShopScene);
    }

    #endregion

    #region Implementation

    private IEnumerator ShowScore(float score, Texture2D tex)
    {
        yield return new WaitForEndOfFrame();

        Cursor.visible = true;
        scoringUIController.Show(GetNumStars(score), tex);
        isTattooComplete = true;

        DataPersistenceManager.instance.SaveGame();
    }

    private int GetNumStars(float score)
    {
        int numStars = 0;

        switch(score)
        {
            case > 90f:
                numStars = 4;
                break;
            case > 85f:
                numStars = 3;
                break;
            case > 75f:
                numStars = 2;
                break;
            case > 65f:
                numStars = 1;
                break;
        }

        return numStars;
    }

    #endregion
}