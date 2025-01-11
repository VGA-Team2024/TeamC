using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyWaveNeedle : MonoBehaviour
{
    private EnemyWaveNeedleAttack _waveAttack;
    private Animator _animator;
    private CancellationTokenSource _tokenSource = new CancellationTokenSource();
    
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        _tokenSource ??= new CancellationTokenSource();
        Wave().Forget();
    }

    private void OnDestroy()
    {
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
    }

    private async UniTask Wave()
    {
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f, cancellationToken : _tokenSource.Token);
        _waveAttack.ReturnObject(gameObject);
    }

    public void GetWaveAttack(EnemyWaveNeedleAttack attack)
    {
        _waveAttack = attack;
    }
}
