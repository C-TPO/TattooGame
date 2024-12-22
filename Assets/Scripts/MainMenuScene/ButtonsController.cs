using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ButtonsController : MonoBehaviour
{
    [SerializeField, NotNull] private GenericPopupController genericPopupController = null;
    [SerializeField, NotNull] private SettingsPopupController settingsPopupController = null;
    [SerializeField, NotNull] private GameObject continueButton = null;

    private void Start()
    {
        continueButton.SetActive(DataPersistenceManager.instance.HasSavedData());
    }

    public void OnNewGameClick()
    {
        if(!DataPersistenceManager.instance.HasSavedData())
        {
            StartNewGame();
            return;
        }

        string message = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", "ui_newgameareyousure");
        genericPopupController.Open(message, StartNewGame);

        return;

        void StartNewGame()
        {
            DataPersistenceManager.instance.NewGame();
            SceneLoader.Load(SceneLoader.GameScene.ShopScene);
        }
    }

    public void OnContinueClick()
    {
        if(!DataPersistenceManager.instance.HasSavedData())
            return;
        
        SceneLoader.Load(SceneLoader.GameScene.ShopScene);
    }

    public void OnSettingsClick()
    {
        settingsPopupController.Show();
    }

    public void OnExitClicked()
    {
        string message = LocalizationSettings.StringDatabase.GetLocalizedString("MainMenu", "ui_areyousureyouwanttoexit");
        genericPopupController.Open(message, Application.Quit);
    }
}
