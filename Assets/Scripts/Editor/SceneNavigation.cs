using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public static class SceneNavigation
{
    [MenuItem("Scene/TitleScene")]
    private static void Scene0()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(0);
    }
    
    [MenuItem("Scene/GameMain")]
    private static void Scene1()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(1);
    }

    [MenuItem("Scene/ResultScene")]
    private static void Scene2()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(2);
    }
    private static void OpenScene(int sceneIndex)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);

        if (!string.IsNullOrEmpty(scenePath))
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}