using System;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

using Random = UnityEngine.Random;

public class TattooSceneController : MonoBehaviour, IDataPersistence
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
        currentClient = new TattooClient();//REMOVE
        currentClient.Init(new TattooClientData("Pierre"));//REMOVE
        
        if(stencils.Length > 0)//REMOVE
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
        tattoosCompleted++;
        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        tattoosCompleted = data.tattoosCompleted;
    }

    public void SaveData(GameData data)
    {
        data.tattoosCompleted = tattoosCompleted;
    }

    public void TempBack()
    {
        SceneLoader.Load(SceneLoader.GameScene.MainMenuScene);
    }

    #endregion
}