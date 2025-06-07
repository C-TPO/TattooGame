using UnityEngine;
using UnityEngine.Events;

public class ClickableSprite : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick;

    void OnMouseDown()
    {
        print("ClickableSprite clicked: " + gameObject.name);
        onClick?.Invoke();
    }
}
