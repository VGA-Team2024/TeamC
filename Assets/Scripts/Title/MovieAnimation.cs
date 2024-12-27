using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Title;
using UnityEngine;
using UnityEngine.Video;

public class MovieAnimation : MonoBehaviour
{
    [SerializeField, InspectorVariantName("Movieコンポーネント")] private VideoPlayer _moviePlayer;

    [SerializeField, InspectorVariantName("fairyのエフェクト")] private EffectAnimation _fairy;

    [SerializeField,InspectorVariantName("妖精を出す間隔")] private float _duration;
    
    [SerializeField] private OpeningTitleText _titleText;

    private CancellationTokenSource _cts;
    
    /// <summary>動画を読み込む</summary>
    public async UniTask PrepareMovie()
    {
        _moviePlayer.Prepare();
    
        // 動画の準備が完了するまで待機
        while (!_moviePlayer.isPrepared)
        {
            await UniTask.Yield();
        }
    }

    /// <summary>Movieアニメーションを再生する</summary>
    public async UniTask StartMovieAnimation(CancellationToken ct)
    {
        // キャンセル用のトークンソースを作成
        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        // ビデオ再生
        _moviePlayer.loopPointReached += FinishMovie;
        
        // 再生開始
        await PlayMovie(_cts.Token);

        // イベントを解除
        _moviePlayer.loopPointReached -= FinishMovie;
    }

    // ビデオ再生を開始し、終了またはキャンセルを待機
    private async UniTask PlayMovie(CancellationToken ct)
    {
        while (!_moviePlayer.isPrepared)
        {
            await UniTask.Yield();
        }

        _moviePlayer.Play();
        await _titleText.ShowTitle();
        
        // エフェクトの再生をスケジュール
        _ = PlayEffectAfterDelay(_duration, ct);

        try
        {
            // 終了イベントを待機
            UniTaskCompletionSource tcs = new();

            // ビデオ終了時にタスクを完了する
            _moviePlayer.loopPointReached += _ => tcs.TrySetResult();

            // キャンセル可能なタスクで待機
            await UniTask.WhenAny(tcs.Task, UniTask.WaitUntilCanceled(ct));

            // キャンセルされた場合に例外をスロー
            ct.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("再生がキャンセルされました");
            _moviePlayer.Stop();
        }
    }

    // ムービー再生終了の処理
    private void FinishMovie(VideoPlayer vp)
    {
        // 再生が終了したらシーンを遷移する
        SceneLoader.LoadSceneSimple("Stage1_FairyForest");
    }

    private async UniTask PlayEffectAfterDelay(float delaySeconds,CancellationToken ct)
    {
        try
        {
            // n秒間
            await UniTask.Delay(TimeSpan.FromSeconds(delaySeconds),cancellationToken: ct);
            
            _fairy.AddCreate();
            
            // エフェクトを再生
            await _fairy.MoveAnimation();
            
            // エフェクトを消す
            _fairy.gameObject.SetActive(false);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("エフェクト再生がキャンセルされた");
        }
    }
    
    // ムービーをスキップさせたい時は必ず呼ぶ必要がある
    private void Cancel()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}