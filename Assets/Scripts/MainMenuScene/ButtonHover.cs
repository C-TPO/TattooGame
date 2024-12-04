using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    [SerializeField, NotNull] private TextMeshProUGUI text = null;
    [SerializeField] private Color highlightColor = Color.white;

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
