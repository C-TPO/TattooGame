using System;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

using Random = UnityEngine.Random;

public class SceneController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private SpriteRenderer stencil = null;
    [SerializeField] private DrawManager drawManager = null;
    [SerializeField] private ScoreController scoreController = null;
    [SerializeField] private Sprite[] stencils = {};
    
    private TattooClient currentClient;
    private int tattoosCompleted = 0;

    #region Unity Messages

    void Start()
    {
        //TODO: Client and stencil should be passed in here, after the booking phase
        currentClient = new TattooClient();
        currentClient.Init("Doof McGoof");
        if(stencils.Length > 0)
        {
            stencil.sprite = stencils[Random.Range(0, stencils.Length)];
        }
        drawManager.EnableTattooing(currentClient, stencil);

        Debug.Log(DataPersistenceManager.instance.GameData.tattoosCompleted);
    }

    #endregion

    #region Public API

    public void ValidateTattoo()
    {
        //Wired up in inspector
        scoreController.ScoreTattoo(stencil);
        print("Validate");
        tattoosCompleted++;
        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        tattoosCompleted = data.tattoosCompleted;
    }

    public void SaveData(ref GameData data)
    {
        data.tattoosCompleted = tattoosCompleted;
    }

    public void TempBack()
    {
        SceneLoader.Load(SceneLoader.GameScene.MainMenuScene);
    }

    #endregion
}