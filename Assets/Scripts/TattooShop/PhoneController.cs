using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class PhoneController : MonoBehaviour
{
    [SerializeField] private SettingsPopupController settingsPopupController = null;
    [SerializeField] private BookingUIController bookingUI = null;

    private Animator animator;
    private bool isShown = false;
    private bool canToggle = true;

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

    //--------------------
    //APP ICON TAP METHODS
    //--------------------

    public void OpenBookingApp()
    {
        bookingUI.Show();
    }

    public void OpenSocialMediaApp()
    {
        //TODO
    }

    public void OpenTattooShopInfoApp()
    {
        //TODO
    }

    public void OpenShopApp()
    {
        //TODO
    }

    public void OpenHiringApp()
    {
        //TODO
    }

    public void OpenSettingsApp()
    {
        settingsPopupController.Show();
    }
    
    public void ExitApp()
    {
        //TODO: Add an 'are you sure' popup
        SceneLoader.Load(SceneLoader.GameScene.MainMenuScene);
    }

    #endregion

    #region Implementation

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
