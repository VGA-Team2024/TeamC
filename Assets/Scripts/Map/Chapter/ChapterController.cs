using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterController : MonoBehaviour
{
    [SerializeField, InspectorVariantName("遷移先のシーン名")] private string _gameSceneName;
    [SerializeField, InspectorVariantName("対応するインゲーム内のボタン名")] private string _buttonName;
    [SerializeField, InspectorVariantName("はじめからでムービーを流したい場合")] private bool _start;
    [SerializeField] private GameObject _chapterCanvas;
    [SerializeField] private MovieAnimation _movieAnimation;
    [SerializeField] private UIButton _stageSelectButton; // 自身に付いたもの
    
    private CancellationTokenSource _cts;

    void Start()
    {
        _cts = new CancellationTokenSource();
        Initialize();
        // はじめから
        if (_start)
        {
            _stageSelectButton.OnClickAddListener(() => UniTask.Void(async () => await HandleFromStartButtonClick()));
        }
        // チャプター選択
        else
        {
            _stageSelectButton.OnClickAddListener(() => UniTask.Void(async () => await LoadMapData()));
        }
    }
    
    // シーン(インゲーム)の同時読み込みとマップ移動を行い、シーンを遷移する
    private async UniTask LoadMapData()
    {
        var asyncLoad = SceneManager.LoadSceneAsync(_gameSceneName, LoadSceneMode.Additive);
        await UniTask.WaitUntil(() => asyncLoad.isDone); // アクティブ化→シーン切り替えが早すぎるとエラーが出るため必要

        SetChapter();
        
        // 現在のシーンを解放し、シーンの親を切り替える
        var currentScene = SceneManager.GetActiveScene();
        var loadedScene = SceneManager.GetSceneByName(_gameSceneName);

        if (loadedScene.IsValid())
        {
            SceneManager.SetActiveScene(loadedScene);
            await SceneManager.UnloadSceneAsync(currentScene);
        }
        else
        {
            Debug.LogWarning("シーン切り替えに失敗しました");
        }
    }

    private void Initialize()
    {
        _chapterCanvas = transform.parent.gameObject;
        _movieAnimation = FindObjectOfType<MovieAnimation>();
        if (_movieAnimation == null)
        {
            Debug.LogWarning("MovieAnimationが取得できませんでした");
        }
        _stageSelectButton = GetComponent<UIButton>();
    }
    
    // 「はじめから」を押した時の処理
    private async UniTask HandleFromStartButtonClick()
    {
        _chapterCanvas.SetActive(false);
        await _movieAnimation.StartMovieAnimation(_cts.Token); // オープニングムービーを流す
    }

    // インゲーム内のチャプターパネルから情報を取得、マップ切り替えを行う
    private void SetChapter()
    {
        var mapManager = FindObjectOfType<MapManager>();
        if (mapManager == null)
        {
            Debug.LogWarning($"ボタンの親となるMapManagerが見つかりません");
            return;
        }
        mapManager.CanvasActivateFlag = true;
        
        var mapSetButton = GameObject.Find(_buttonName);
        if (mapSetButton == null)
        {
            Debug.LogWarning($"「{_buttonName}」は存在しません。正しい名前を入力してください");
        }
        var mapSetter = mapSetButton.GetComponent<MapSetter>();
        mapSetter.SkipMap(); // マップ切り替え
    }
}
