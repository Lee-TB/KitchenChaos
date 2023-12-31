using System;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());        
    }

    public static void ReloadScene()
    {
        if(Enum.TryParse<Scene>(SceneManager.GetActiveScene().name, out targetScene))
        {
            Load(targetScene);
        }        
    }
}
