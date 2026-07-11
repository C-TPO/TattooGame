using UnityEngine;

[CreateAssetMenu(fileName = "TattooLineBrush", menuName = "Tattoo/Line Brush")]
public class TattooBrushDefinition : ScriptableObject
{
    [SerializeField, Min(0.001f)] private float size = 0.02f;
    [SerializeField, Range(0f, 1f)] private float opacity = 1f;
    [SerializeField, Range(0.01f, 1f)] private float spacing = 0.15f;
    [SerializeField, Range(0f, 1f)] private float hardness = 0.95f;
    [SerializeField] private Color color = Color.black;
    [SerializeField, Min(0f)] private float painPerSecond = 1f;

    public float Size => size;
    public float Opacity => opacity;
    public float Spacing => spacing;
    public float Hardness => hardness;
    public Color Color => color;
    public float PainPerSecond => painPerSecond;
}
