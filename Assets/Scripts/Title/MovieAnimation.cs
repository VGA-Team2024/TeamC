using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

public class MovieAnimation : MonoBehaviour
{
    [SerializeField, InspectorVariantName("Movieコンポーネント")]
    private VideoPlayer _moviePlayer;

    private CancellationTokenSource _cts;

    private async UniTask Start()
    {
        _cts = new CancellationTokenSource();
        await StartMovieAnimation(_cts.Token);
    }

    /// <summary>Movieアニメーションを再生する</summary>
    private async UniTask StartMovieAnimation(CancellationToken ct)
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
        _moviePlayer.Prepare();
        while (!_moviePlayer.isPrepared)
        {
            await UniTask.Yield();
        }

        _moviePlayer.Play();

        try
        {
            // 終了イベントを待機
            var tcs = new UniTaskCompletionSource();

            // ビデオ終了時にタスクを完了する
            _moviePlayer.loopPointReached += _ => tcs.TrySetResult();

            // キャンセル可能なタスクで待機
            await UniTask.WhenAny(tcs.Task, UniTask.WaitUntilCanceled(ct));

            // キャンセルされた場合に例外をスロー
            ct.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException)
        {
            Debug.Log("再生がキャンセルされました");
            _moviePlayer.Stop();
        }
    }

    // ムービー再生終了の処理
    private void FinishMovie(VideoPlayer vp)
    {
        // 再生が終了したらシーンを遷移する
        SceneLoader.LoadSceneSimple("Stage1_FairyForest");
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