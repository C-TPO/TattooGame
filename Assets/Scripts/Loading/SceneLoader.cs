using System;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum GameScene
    {
        MainMenuScene,
        LoadingScene,
        ShopScene,
        TattooScene,
    }

    private static Action onLoaderCallback;

    public static void Load(GameScene scene)
    {
        onLoaderCallback = () => 
        {
             SceneManager.LoadScene(scene.ToString());
        };

        SceneManager.LoadScene(GameScene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        onLoaderCallback?.Invoke();
        onLoaderCallback = null;
    }
}
