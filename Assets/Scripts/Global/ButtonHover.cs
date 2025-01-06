using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ButtonHover : MonoBehaviour
{
    [SerializeField, NotNull] private TextMeshProUGUI text = null;
    [SerializeField] private Color highlightColor = new Color(0.984313725f, 0f, 0f);

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
    }

    public void OnHoverEnd()
    {
        text.color = defaultColor;
    }
}
