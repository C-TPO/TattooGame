using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PhoneController : MonoBehaviour
{
    [SerializeField] private GameObject appsContainer = null;
    [SerializeField] private GameObject fadedBackgroundObject = null;
    [SerializeField] private GameObject uiObject = null;
    [SerializeField] private SettingsPopupController settingsPopupController = null;
    [SerializeField] private BookingUIController bookingUI = null;

    private Animator animator;
    private PhoneApp selectedButton = null;
    private bool isShown = false;
    private bool isZoomedIn = false;
    private bool canToggle = true;

    #region Unity Messages

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!isShown)
            return;
        
        if(Input.GetKeyDown(KeyCode.Escape))
            CloseTapped();
    }

    #endregion

    #region Public API

    public void TogglePhone()
    {
        if(!canToggle)
            return;
        
        canToggle = false;
        isShown = !isShown;

        if (isShown)
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
                ExitGame();
                break;
            default:
                break;
        }
    }

    public void CloseCurrentApp()
    {
        if(selectedButton != null)
        {
            switch(selectedButton.Type)
            {
                case PhoneApp.AppType.Booking:
                    bookingUI.Hide();
                    break;
                case PhoneApp.AppType.Hiring:
                    break;
                case PhoneApp.AppType.Shop:
                    break;
                case PhoneApp.AppType.Social:
                    break;
                case PhoneApp.AppType.Info:
                    break;
                case PhoneApp.AppType.Settings:
                    break;
            }

            selectedButton.ShrinkButton();
            selectedButton = null;
        }
        
        ZoomOut();
    }

    public void CloseTapped()
    {
        if(isZoomedIn)
        {
            CloseCurrentApp();
        }
        else
        {
            TogglePhone();
        }
    }

    #endregion

    #region Implementation

    //--------------------
    //APP ICON TAP METHODS
    //--------------------

    private void OpenBookingApp()
    {
        ZoomIn();
        bookingUI.Show();
    }

    private void OpenSocialMediaApp()
    {
        ZoomIn();
        //TODO
    }

    private void OpenTattooShopInfoApp()
    {
        ZoomIn();
        //TODO
    }

    private void OpenShopApp()
    {
        ZoomIn();
        //TODO
    }

    private void OpenHiringApp()
    {
        ZoomIn();
        //TODO
    }

    private void OpenSettingsApp()
    {
        ZoomIn();
        //settingsPopupController.Show();
    }
    
    private void ExitGame()
    {
        //TODO: Add an 'are you sure' popup
        SceneLoader.Load(SceneLoader.GameScene.MainMenuScene);
    }

    private void Show()
    {
        animator.Play("Open");
        appsContainer.SetActive(true);
    }

    private void Hide()
    {
        animator.Play("Close");
        appsContainer.SetActive(false);
        fadedBackgroundObject.SetActive(false);
        uiObject.SetActive(true); 
        //TODO: if an app is open, close it here. New close animation needed?
    }

    private void ZoomIn()
    {
        canToggle = false;
        animator.Play("AppOpen");
        appsContainer.SetActive(false);
        isZoomedIn = true;
        fadedBackgroundObject.SetActive(true);
        uiObject.SetActive(false);
    }

    private void ZoomOut()
    {
        canToggle = false;
        animator.Play("AppClose");
        appsContainer.SetActive(true);
        isZoomedIn = false;
        fadedBackgroundObject.SetActive(false);
        uiObject.SetActive(true);   
    }

    #endregion
}
