using UnityEngine;

public struct TattooBrushSample
{
    public Vector2 uv;
    public Vector2 direction;
    public float speed;
    public float size;
    public float opacity;
    public Color color;

    public TattooBrushSample(
        Vector2 uv,
        Vector2 direction,
        float speed,
        float size,
        float opacity,
        Color color)
    {
        this.uv = uv;
        this.direction = direction;
        this.speed = speed;
        this.size = size;
        this.opacity = opacity;
        this.color = color;
    }
}
