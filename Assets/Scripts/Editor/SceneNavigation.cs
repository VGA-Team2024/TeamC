using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

// メニューバーにシーンを切り替えるGUIを表示
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
    
    [MenuItem("Scene/testmap01")]
    private static void Scene3()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(3);
    }
    
    [MenuItem("Scene/Stage1_FairyForest")]
    private static void Scene4()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(4);
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