using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour
{
    [SerializeField, NotNull] private GenericPopupController genericPopupController = null;
    [SerializeField, NotNull] private SettingsPopupController settingsPopupController = null;

    public void OnNewGameClick()
    {
        //TODO: SETUP NEW GAME SYSTEM
    }

    public void OnContinueClick()
    {
        //TODO: CONTINUE LAST PLAYED GAME
        SceneLoader.Load(SceneLoader.GameScene.TattooScene);
    }

    public void OnSettingsClick()
    {
        settingsPopupController.Show();
    }

    public void OnExitClicked()
    {
        genericPopupController.Open(Application.Quit);
    }
}
