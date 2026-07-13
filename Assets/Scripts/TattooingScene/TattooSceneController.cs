using System.Collections;
using UnityEngine;

public class TattooSceneController : MonoBehaviour, IDataPersistence
{
    [Header("Scene")]
    [SerializeField] private SpriteRenderer drawingArea = null;
    [SerializeField] private SpriteRenderer stencil = null;
    [SerializeField] private TattooSurface tattooSurface = null;
    [SerializeField] private TattooInputController tattooInputController = null;
    [SerializeField] private TattooStrokeController tattooStrokeController = null;
    [SerializeField] private PainToleranceMeter painMeter = null;

    [Header("Scoring")]
    [SerializeField] private TattooScoringMode scoringMode;
    [SerializeField] private ScoreController scoreController = null;
    [SerializeField] private ScoreControllerLegacy scoreControllerLegacy = null;
    [SerializeField] private ScoringUIController scoringUIController = null;

    private TattooClientBookingData currentClientData = null;
    private TattooStencilScriptableObject currentStencilData = null;
    private bool isTattooComplete = false;

    public enum TattooScoringMode
    {
        LegacyPixel,
        DistanceBased
    }

    #region Unity Messages

    private void Start()
    {
        currentClientData = DataPersistenceManager.instance.GameData.currentBookedClient;

        currentStencilData = TattooStencilManager.instance.GetStencilByIndex(currentClientData.tattooDesignIndex);

        stencil.sprite = currentStencilData.sprite;

        tattooSurface.Initialize(drawingArea,stencil);

        tattooInputController.Initialize(drawingArea);
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

        Texture2D tattooTexture = tattooSurface.CreateTexture2D();

        TattooScoreResult scoreResult = ScoreTattoo(tattooTexture);

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

    private TattooScoreResult ScoreTattoo(Texture2D tattooTexture)
    {
        Texture2D targetTexture =
            TattooScoringTextureBuilder.CreateTargetTexture(
                stencil,
                drawingArea,
                tattooTexture.width,
                tattooTexture.height
            );

        TattooScoreResult scoreResult;

        switch (scoringMode)
        {
            case TattooScoringMode.LegacyPixel:
                scoreResult =
                    scoreControllerLegacy.ScoreTattoo(
                        tattooTexture,
                        targetTexture
                    );

                    // For testing purposes, also score with the new scoring system
                    scoreController.ScoreTattoo(
                        tattooTexture,
                        targetTexture
                    );
                break;

            case TattooScoringMode.DistanceBased:
                scoreResult =
                    scoreController.ScoreTattoo(
                        tattooTexture,
                        targetTexture
                    );

                    // For testing purposes, also score with the legacy scoring system
                    scoreControllerLegacy.ScoreTattoo(
                        tattooTexture,
                        targetTexture
                    );
                break;

            default:
                scoreResult = default;
                break;
        }

        Destroy(targetTexture);

        return scoreResult;
    }

    private IEnumerator ShowScore(TattooScoreResult scoreResult,Texture2D tattooTexture)
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

    private float GetPainMultiplier(TattooClientData.ClientTolerances tolerance)
    {
        switch (tolerance)
        {
            case TattooClientData.ClientTolerances.VeryLow:
                return 60f;

            case TattooClientData.ClientTolerances.Low:
                return 40f;

            case TattooClientData.ClientTolerances.Medium:
                return 25f;

            case TattooClientData.ClientTolerances.High:
                return 18f;

            case TattooClientData.ClientTolerances.VeryHigh:
                return 10f;

            default:
                return 25f;
        }
    }

    private float GetRecoveryPerSecond(TattooClientData.ClientTolerances recovery)
    {
        switch (recovery)
        {
            case TattooClientData.ClientTolerances.VeryLow:
                return 4f;

            case TattooClientData.ClientTolerances.Low:
                return 8f;

            case TattooClientData.ClientTolerances.Medium:
                return 15f;

            case TattooClientData.ClientTolerances.High:
                return 20f;

            case TattooClientData.ClientTolerances.VeryHigh:
                return 30f;

            default:
                return 15f;
        }
    }

    #endregion
}
