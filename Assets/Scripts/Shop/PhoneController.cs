using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PhoneController : MonoBehaviour
{
    [SerializeField] private SettingsPopupController settingsPopupController = null;

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

    #endregion

    #region Implementation

    public void ResetToggleable()
    {
        canToggle = true;
    }

    private void Show()
    {
        animator.Play("Open");
    }

    private void Hide()
    {
        animator.Play("Close");
    }

    //APP ICON TAP METHODS

    public void OpenSettings()
    {
        settingsPopupController.Show();
    }

    #endregion
}
