using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン再生をやりやすくするデバッグ
/// </summary>
[InitializeOnLoad]
public static class ScenePlayer
{
    /// <summary>
    /// コンストラクタ
    /// NOTE: InitializeOnLoad属性によりエディター起動時に呼び出される
    /// </summary>
    static ScenePlayer()
    {
        EditorApplication.playModeStateChanged += OnChangedPlayMode;
    }

    //プレイモードが変更された
    private static void OnChangedPlayMode(PlayModeStateChange state)
    {
        //エディタの実行が開始された時に、最初のシーンをnullにする(普通の再生ボタンを押した時に使われないように)
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            EditorSceneManager.playModeStartScene = null;
        }

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            Play();
        }
    }

    /// <summary>
    /// 現在のシーンチェックして、マージが必要なシーンを足す
    /// </summary>
    public static void Play()
    {
        //シーンデータベースから現在のシーンが登録されているか確認してもらい、登録されていたらそのシーンで再生する
        SceneLoader.PlayEditor();

        //登録されてない場合はそのまま再生する
        EditorApplication.isPlaying = true;
    }
}