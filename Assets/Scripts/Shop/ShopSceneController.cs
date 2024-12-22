using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSceneController : MonoBehaviour
{
    [SerializeField] private PhoneController phoneController;

    public void OnPhoneToggled()
    {
        phoneController.TogglePhone();
    }
}
