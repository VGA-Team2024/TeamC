using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#if USE_ADDRESSABLES
using UnityEngine.AddressableAssets;
#endif

public class SceneLoader
{
    static public void Load()
    {
        //

        //
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
        var setting = GameSettings.GetSetting(sceneName);

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
