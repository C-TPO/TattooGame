using System;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer stencil = null;
    [SerializeField] private DrawManager drawManager = null;
    [SerializeField] private ScoreController scoreController = null;

    #region Unity Messages

    void Start()
    {
        drawManager.EnableTattooing(stencil);
    }

    #endregion

    #region Public API

    public void ValidateTattoo()
    {
        //Wired up in inspector
        scoreController.ScoreTattoo(stencil);
    }

    #endregion
}