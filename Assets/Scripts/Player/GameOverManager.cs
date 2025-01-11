using System;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
public class GameOverManager : SingletonMonoBehaviour<GameOverManager>
{
    public Player Player;
    private RespawnPoint _point;
    [SerializeField]FadeController _fadeController;
    [SerializeField,InspectorVariantName("すべての処理が終わる時間")] float _fadeDuration = 1f;
    
    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField,InspectorVariantName("リトライボタン")] private UIButton _retryButton;
    [SerializeField,InspectorVariantName("レビューボタン")] private UIButton _reviewButton;
    
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    public RespawnPoint Point
    {
        set => _point = value;
    }

    private void Start()
    {
        GameEventRecorder.GameStart();
    }

    private void OnEnable()
    {
        _retryButton.OnClickAddListener(ReTry);
        _retryButton.OnClickAddListener(() => _gameOverCanvas.SetActive(false));
        _reviewButton.OnClickAddListener(() => GameEventRecorder.GameReview(ReTry));
        _reviewButton.OnClickAddListener(() => _gameOverCanvas.SetActive(false));
    }

    public async void GameOver()
    {
        
        Player.PlayerMove.IsFreeze = (true, true);
        _fadeController.FadeOut(_fadeDuration/2);
        await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration/2), cancellationToken: _cts.Token);
        _gameOverCanvas.SetActive(true);
        if (Player.PlayerMove.TryGetComponent<ITeleportable>(out ITeleportable t))
        {
            t.Teleport(_point.transform.position);
        }
        Player.PlayerStatus.GameOver();
    }

    private void ReTry()
    {
        _fadeController.FadeIn(_fadeDuration/2);
        Player.PlayerMove.IsFreeze = (false, false);
        Player.PlayerStatus.GodModeEnd();
    }

    private void OnDestroy()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
