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
    [SerializeField] private Sprite[] stencils;//TODO: REMOVE!

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
        btn = GetComponent<Button>();
        tattooClientBookingData = bookingData;
        //TODO: redo this
        nameText.text = tattooClientBookingData.clientData.clientName;
        stencilImg.sprite = stencils[bookingData.tattooDesignIndex-1];
        stencilImg.SetNativeSize();
        btn.onClick.AddListener(() => buttonCallback(this));
    }

    #endregion
}
