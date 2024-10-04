using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

/// <summary>
/// シーンローダー
/// NOTE: シーンの読み込みを管理する
/// </summary>
public class SceneLoader
{
#if UNITY_EDITOR
    public static bool PlayEditor()
    {
        SceneDependencies sceneDB = AssetDatabase.LoadAssetAtPath<SceneDependencies>("Assets/" + SceneDependencies.SOAssetPath);
        var current = sceneDB.Get(SceneManager.GetActiveScene().name);

        //通常再生の場合はそのまま帰る
        if (current.SceneType == SceneType.Normal)
            return true;

        //依存シーンがある場合はベースシーンを拾ってきて再生する
        var setting = GameSettings.GetSetting(current.SceneType.ToString());

        //エディタ実行時の最初のシーンに取得したシーンを設定
        //@PlaySceneの場合はコールされたシーンをベースにする
        SceneDependencies.Dependencies baseScene;
        if (setting.BaseSceneName == "@PlayScene")
        {
            baseScene = current;
        }
        else
        {
            baseScene = sceneDB.Get(setting.BaseSceneName);
        }
        if (baseScene == null)
        {
            Debug.LogError(current.Name + "の再生に必要なシーンがありません");
            return false;
        }

        SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(baseScene.AssetPath);
        if (sceneAsset == null)
        {
            Debug.LogError(baseScene.Name + "というシーンアセットは存在しません");
            return false;
        }

        EditorSceneManager.playModeStartScene = sceneAsset;

        //エディタの再生開始
        EditorApplication.isPlaying = true;

        //追加シーンの読み込みをする＆待つ
        foreach (var addScene in setting.AdditiveSceneName)
        {
            SceneManager.LoadScene(addScene, LoadSceneMode.Additive);
        }

        return true;
    }
#endif

    // 制約
    // Editorでこのstaticクラスを作ってはいけない

    static SceneDependencies _sceneDependencies = new SceneDependencies();


    static public void Load()
    {
        //

        //
    }

    static public void CheckScene()
    {
        Debug.Log("Check");
    }



    static void SceneInit()
    {
        //現在のシーンの初期化処理をする
        var execArrayInit = GameObject.FindObjectsOfType<GameExecuterBase>();
        foreach (var exec in execArrayInit)
        {
            exec.InitializeScene();
        }
    }

    static void SceneTerm()
    {
        //現在のシーンの解放処理をする
        var execArrayTerm = GameObject.FindObjectsOfType<GameExecuterBase>();
        foreach (var exec in execArrayTerm)
        {
            exec.FinalizeScene();
        }
    }



    /// <summary>
    /// シーン呼び出し
    /// NOTE: 単一のシーンを呼び出す
    /// </summary>
    /// <param name="sceneName"></param>
    static public async void LoadSceneSimple(string sceneName)
    {
        //ベースシーンの読み込みをする＆待つ
        var baseSceneHandle = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        await UniTask.WaitUntil(() => baseSceneHandle.isDone);
    }


    /// <summary>
    /// シーン呼び出し
    /// NOTE: LoadSceneMode.Additiveを使用し、複数のシーンを結合する
    /// </summary>
    /// <param name="sceneName"></param>
    static public async void LoadScene(string sceneName)
    {
        if (_sceneDependencies == null)
        {
            var handle = Addressables.LoadAssetAsync<SceneDependencies>(SceneDependencies.SOAssetPath);
            await UniTask.WaitUntil(() => handle.IsDone);
            _sceneDependencies = handle.Result;
        }

        var sceneType = _sceneDependencies.Get(sceneName);
        var setting = GameSettings.GetSetting(sceneType.SceneType.ToString());

        SceneTerm();

#if USE_ADDRESSABLES
        //TBD
#else
        //ベースシーンの読み込みをする＆待つ
        var baseSceneHandle = SceneManager.LoadSceneAsync(setting.BaseSceneName, LoadSceneMode.Single);
        await UniTask.WaitUntil(() => baseSceneHandle.isDone);

        //追加シーンの読み込みをする＆待つ
        List<AsyncOperation> handles = new List<AsyncOperation>();
        foreach(var addScene in setting.AdditiveSceneName)
        {
            handles.Add(SceneManager.LoadSceneAsync(addScene, LoadSceneMode.Additive));
        }
        await UniTask.WaitUntil(() => handles.All(h => h.isDone));
#endif

        SceneInit();
    }
}
