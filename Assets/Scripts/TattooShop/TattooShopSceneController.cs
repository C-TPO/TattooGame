using TMPro;
using UnityEngine;

public class TattooShopSceneController : MonoBehaviour
{
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private GameObject tattooButton;
    [SerializeField] private BookingUIController bookingUIController;
    [SerializeField] private TextMeshProUGUI moneyText;

    #region Unity Messages

    private void Start()
    {
        //TODO: possibly move this depending on when we clear out the booked tattoo
        tattooButton.SetActive(DataPersistenceManager.instance.HasBookedTattoo());
        moneyText.text = DataPersistenceManager.instance.GameData.inventory.totalCash.ToString();
    }

    private void OnEnable()
    {
        bookingUIController.OnClientBooked += HandleClientBooked;
    }

    #endregion

    #region Public API

    public void OnPhoneToggled()
    {
        phoneController.TogglePhone();
    }

    public void OnTattooButtonPressed()
    {
        if(DataPersistenceManager.instance.HasBookedTattoo())
            SceneLoader.Load(SceneLoader.GameScene.TattooScene);
        else
            tattooButton.SetActive(false);
    }

    #endregion

    #region Implementation

    private void HandleClientBooked()
    {
        tattooButton.SetActive(DataPersistenceManager.instance.HasBookedTattoo());
        phoneController.TogglePhone();
    }

    #endregion
}
