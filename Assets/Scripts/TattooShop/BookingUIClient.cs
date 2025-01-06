using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[RequireComponent(typeof(Button))]
public class BookingUIClient : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image clientImg;
    [SerializeField] private Image stencilImg;
    [SerializeField] private TextMeshProUGUI hoursText;

    private TattooClientBookingData tattooClientBookingData;
    private Button btn = null;

    #region Unity Messages

    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }

    #endregion

    #region Public API

    public TattooClientBookingData TattooClientBookingData => tattooClientBookingData;
    public string ClientName => nameText.text;

    public void Init(TattooClientBookingData bookingData, Action<BookingUIClient> buttonCallback)
    {
        TattooStencilScriptableObject currentStencil = TattooStencilManager.instance.GetStencilByIndex(bookingData.tattooDesignIndex);
        btn = GetComponent<Button>();
        tattooClientBookingData = bookingData;
        nameText.text = tattooClientBookingData.clientData.clientName;
        hoursText.text = currentStencil.duration.ToString();
        stencilImg.sprite = currentStencil.sprite;
        stencilImg.SetNativeSize();
        btn.onClick.AddListener(() => buttonCallback(this));
    }

    #endregion
}
