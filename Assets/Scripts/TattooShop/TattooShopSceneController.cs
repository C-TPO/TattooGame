using UnityEngine;

public class TattooShopSceneController : MonoBehaviour
{
    [SerializeField] private PhoneController phoneController;

    public void OnPhoneToggled()
    {
        phoneController.TogglePhone();
    }
}
