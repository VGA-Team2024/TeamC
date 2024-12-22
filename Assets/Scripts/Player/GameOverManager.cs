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
    
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    public RespawnPoint Point
    {
        set => _point = value;
    }
    
    public async void GameOver()
    {
        Player.PlayerMove.IsFreeze = (true, true);
        _fadeController.FadeOut(_fadeDuration/2);
        await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration/2), cancellationToken: _cts.Token);
        if (Player.PlayerMove.TryGetComponent<ITeleportable>(out ITeleportable t))
        {
            t.Teleport(_point.transform.position);
        }
        Player.PlayerStatus.GameOver();
        _fadeController.FadeIn(_fadeDuration/2);
        await UniTask.Delay((TimeSpan.FromSeconds(_fadeDuration/2)),cancellationToken: _cts.Token);
        Player.PlayerMove.IsFreeze = (false, false);
    }

    private void OnDestroy()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
