using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager instance {get; private set;}

    [Header("Debugging")]
    [SerializeField] private bool initDataIfNull = false;

    [SerializeField]private string fileName = "SaveData.game";
    [SerializeField] private bool useEncryption = false;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    #region Unity Messages

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("FOUND MULTIPLE DATA PERSISTENCE MANAGER IN SCENE! DESTROYING NEWEST INSTANCE.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion

    #region Public API

    public GameData GameData => gameData; 

    public bool HasSavedData() => gameData != null;

    public bool HasBookedTattoo() => gameData.currentBookedClient.clientData.clientName != "";

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
        SaveGame();
    }

    public void LoadGame()
    {
        //load any saved data from a file using the data handler
        gameData = dataHandler.Load();

        if(gameData == null && initDataIfNull)
        {
            NewGame();
        }

        //if no data can be loaded, initialize to a new game
        if(gameData == null)
        {
            Debug.Log("No data was found.");
            return;
        }
        
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        //push the loaded data to all other scripts that need it
        foreach(IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        //pass the data to other scripts so they can update it
        foreach(IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(gameData);
        }

        //save that data to a file using the data handler
        dataHandler.Save(gameData);
    }

    #endregion

    #region Implementation()
    
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    #endregion
}
