using System.Collections;
using UnityEngine;

public class TattooSceneController : MonoBehaviour, IDataPersistence
{
    [Header("Scene")]
    [SerializeField] private SpriteRenderer stencil = null;
    [SerializeField] private TattooSurface tattooCanvas = null;
    [SerializeField] private TattooInputController tattooInputController = null;
    [SerializeField] private TattooStrokeController tattooStrokeController = null;
    [SerializeField] private PainToleranceMeter painMeter = null;

    [Header("Scoring")]
    [SerializeField] private ScoreController scoreController = null;
    [SerializeField] private ScoringUIController scoringUIController = null;

    private TattooClientBookingData currentClientData = null;
    private TattooStencilScriptableObject currentStencilData = null;
    private bool isTattooComplete = false;

    #region Unity Messages

    private void Start()
    {
        currentClientData = DataPersistenceManager
            .instance
            .GameData
            .currentBookedClient;

        currentStencilData = TattooStencilManager
            .instance
            .GetStencilByIndex(currentClientData.tattooDesignIndex);

        stencil.sprite = currentStencilData.sprite;

        tattooCanvas.Initialize(stencil);
        tattooInputController.Initialize(stencil);
        painMeter.ResetMeter();

        tattooStrokeController.Initialize(
            GetPainMultiplier(currentClientData.clientData.painSensitivity),
            GetRecoveryPerSecond(currentClientData.clientData.painRecoveryRate)
        );

        tattooStrokeController.EnableTattooing();

        Cursor.visible = false;
    }

    #endregion

    #region Public API

    public void ValidateTattoo()
    {
        if (isTattooComplete)
        {
            return;
        }

        tattooStrokeController.DisableTattooing();

        Texture2D tattooTexture = tattooCanvas.CreateTexture2D();

        TattooScoreResult scoreResult = scoreController.ScoreTattoo(
            tattooTexture,
            stencil.sprite
        );

        isTattooComplete = true;

        StartCoroutine(ShowScore(
            scoreResult,
            tattooTexture
        ));
    }

    public void LoadData(GameData data)
    {
    }

    public void SaveData(GameData data)
    {
        if (!isTattooComplete)
        {
            return;
        }

        data.currentBookedClient = null;
        data.currentTimeElapsed += currentStencilData.duration;
        data.inventory.TotalCash += 100;
    }

    public void ContinuePressed()
    {
        SceneLoader.Load(SceneLoader.GameScene.ShopScene);
    }

    #endregion

    #region Implementation

    private IEnumerator ShowScore(
        TattooScoreResult scoreResult,
        Texture2D tattooTexture)
    {
        yield return new WaitForEndOfFrame();

        Cursor.visible = true;

        scoringUIController.Show(
            GetNumStars(scoreResult.totalScore),
            tattooTexture
        );

        DataPersistenceManager.instance.SaveGame();
    }

    private int GetNumStars(float score)
    {
        switch (score)
        {
            case > 88f:
                return 4;

            case > 80f:
                return 3;

            case > 70f:
                return 2;

            case > 50f:
                return 1;

            default:
                return 0;
        }
    }

    private float GetPainMultiplier(
        TattooClientData.ClientTolerances tolerance)
    {
        switch (tolerance)
        {
            case TattooClientData.ClientTolerances.Low:
                return 2f;

            case TattooClientData.ClientTolerances.Medium:
                return 1f;

            case TattooClientData.ClientTolerances.High:
                return 0.5f;

            default:
                return 0.5f;
        }
    }

    private float GetRecoveryPerSecond(
        TattooClientData.ClientTolerances recovery)
    {
        switch (recovery)
        {
            case TattooClientData.ClientTolerances.Low:
                return 1f;

            case TattooClientData.ClientTolerances.Medium:
                return 1.8f;

            case TattooClientData.ClientTolerances.High:
                return 2.4f;

            default:
                return 1f;
        }
    }

    #endregion
}
