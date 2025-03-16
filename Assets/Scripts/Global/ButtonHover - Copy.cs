using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ButtonHover : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text = null;
    [SerializeField] private Color highlightColor = new Color(0.984313725f, 0f, 0f);
    [SerializeField] private AudioClip hoverClip = null;

    private Color defaultColor = Color.white;
    
    private void Start()
    {
        defaultColor = text.color;
    }

    private void OnDisable()
    {
        OnHoverEnd();
    }

    public void OnHoverStart()
    {
        text.color = highlightColor;
        if(hoverClip)
            SoundManager.instance.PlaySound(hoverClip, transform);
    }

    public void OnHoverEnd()
    {
        text.color = defaultColor;
    }
}
