using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class PhoneController : MonoBehaviour
{
    [SerializeField] private SettingsPopupController settingsPopupController = null;
    [SerializeField] private BookingUIController bookingUI = null;

    private Animator animator;
    private bool isShown = false;
    private bool canToggle = true;
    private PhoneApp selectedButton = null;

    #region Unity Messages

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    #endregion

    #region Public API

    public void TogglePhone()
    {
        if(!canToggle)
            return;
        
        canToggle = false;
        isShown = !isShown;

        if(isShown)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void ResetToggleable()
    {
        //NOTE: Called at the end of show/hide animations
        canToggle = true;
    }

    public void AppTapped(PhoneApp button)
    {
        if(selectedButton != button)
        {
            selectedButton?.ShrinkButton();
            selectedButton = button;
            selectedButton.EnlargeButton();
            return;
        }

        switch(button.Type)
        {
            case PhoneApp.AppType.Booking:
                OpenBookingApp();
                break;
            case PhoneApp.AppType.Hiring:
                OpenHiringApp();
                break;
            case PhoneApp.AppType.Shop:
                OpenShopApp();
                break;
            case PhoneApp.AppType.Social:
                OpenSocialMediaApp();
                break;
            case PhoneApp.AppType.Info:
                OpenTattooShopInfoApp();
                break;
            case PhoneApp.AppType.Settings:
                OpenSettingsApp();
                break;
            case PhoneApp.AppType.Exit:
                ExitApp();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Implementation

    //--------------------
    //APP ICON TAP METHODS
    //--------------------

    private void OpenBookingApp()
    {
        bookingUI.Show();
    }

    private void OpenSocialMediaApp()
    {
        //TODO
    }

    private void OpenTattooShopInfoApp()
    {
        //TODO
    }

    private void OpenShopApp()
    {
        //TODO
    }

    private void OpenHiringApp()
    {
        //TODO
    }

    private void OpenSettingsApp()
    {
        settingsPopupController.Show();
    }
    
    private void ExitApp()
    {
        //TODO: Add an 'are you sure' popup
        SceneLoader.Load(SceneLoader.GameScene.MainMenuScene);
    }

    private void Show()
    {
        animator.Play("Open");
    }

    private void Hide()
    {
        animator.Play("Close");
    }

    #endregion
}
