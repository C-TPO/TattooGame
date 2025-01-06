using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stencil", menuName = "Stencil")]
public class TattooStencilScriptableObject : ScriptableObject
{
    public Sprite sprite;
    public int duration = 2;
    public StencilDifficulty difficulty = 0;

    public enum StencilDifficulty
    {
        VeryEasy,
        Easy,
        Medium,
        Hard,
        VeryHard,
    }
}
