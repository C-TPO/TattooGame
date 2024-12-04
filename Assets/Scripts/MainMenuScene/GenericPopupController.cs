using System;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenericPopupController : MonoBehaviour
{
    [SerializeField, NotNull] private Button confirmBtn = null;
    [SerializeField, NotNull] private Button cancelBtn = null;

    private Action onSuccess = null;
    private Action onCancel = null;

    #region Unity Messages

    private void Start()
    {
        confirmBtn.onClick.AddListener(Confirm);
        cancelBtn.onClick.AddListener(Cancel);
    }

    private void OnDestroy()
    {
        confirmBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
    }

    #endregion

    #region Public API

    public void Open(Action doOnSuccess = null, Action doOnCancel = null)
    {
        onSuccess = doOnSuccess;
        onCancel = doOnCancel;
        OpenPopup();
    }

    public void Close()
    {
        ClosePopup(false);
    }

    #endregion

    #region Implementation

    private void Confirm()
    {
        ClosePopup(true);
    }

    private void Cancel()
    {
        ClosePopup(false);
    }

    private void OpenPopup()
    {
        gameObject.SetActive(true);
    }

    private void ClosePopup(bool success)
    {
        if(success)
            onSuccess?.Invoke();
        else
            onCancel?.Invoke();
        
        onSuccess = onCancel = null;
        gameObject.SetActive(false);
    }

    #endregion
}
