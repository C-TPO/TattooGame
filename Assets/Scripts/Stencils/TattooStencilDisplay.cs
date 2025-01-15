using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TattooStencilDisplay : MonoBehaviour
{
    public TattooStencilScriptableObject stencilData;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = stencilData.sprite;
    }
}
