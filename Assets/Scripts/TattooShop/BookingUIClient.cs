using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

[Serializable]
[RequireComponent(typeof(Button))]
public class BookingUIClient : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI toleranceText;
    [SerializeField] private TextMeshProUGUI recoveryText;
    [SerializeField] private TextMeshProUGUI hoursText;
    [SerializeField] private Image clientImg;
    [SerializeField] private Image stencilImg;

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
        toleranceText.text = tattooClientBookingData.clientData.painSensitivity.ToString();
        recoveryText.text = tattooClientBookingData.clientData.painRecoveryRate.ToString();
        hoursText.text = currentStencil.duration.ToString();

        stencilImg.sprite = currentStencil.sprite;
        stencilImg.SetNativeSize();
        btn.onClick.AddListener(() => buttonCallback(this));
    }

    #endregion
}
