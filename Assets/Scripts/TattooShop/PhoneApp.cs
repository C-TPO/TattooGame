using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PhoneApp : MonoBehaviour
{
    public enum AppType
    {
        None,
        Booking,
        Hiring,
        Shop,
        Social,
        Info,
        Settings,
        Exit,
    }

    [SerializeField] private AppType appType = AppType.None;

    private CanvasGroup canvasGroup = null;

    private readonly Vector3 SmallScale = new Vector3(.95f, .95f, .95f);
    private readonly Vector3 LargeScale = new Vector3(1.1f, 1.1f, 1.1f);

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public AppType Type => appType;

    public void EnlargeButton()
    {
        transform.localScale = LargeScale;
        canvasGroup.alpha = 1.0f;
    }

    public void ShrinkButton()
    {
        transform.localScale = SmallScale;
        canvasGroup.alpha = .65f;
    }
}
