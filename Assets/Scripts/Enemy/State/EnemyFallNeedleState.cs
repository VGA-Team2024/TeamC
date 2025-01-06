using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyFallNeedleState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _fallNeedle = Animator.StringToHash("Attack3");
    private readonly GameObject _fallNeedleCollider;
    private readonly Transform _transform;
    private readonly Rigidbody _rb;
    private readonly float _height = 10f;
    private CancellationTokenSource _tokenSource;
    private readonly EnemySounds _sounds;
    private BoxCollider _boxCollider; // このステート中はplayerとぶつからないようにした
    
    public EnemyFallNeedleState(EnemyBase enemyBase, EnemyFreezeState freezeState, 
        Animator animator, GameObject obj, Transform transform, Rigidbody rb, EnemySounds sounds, GameObject gameObject)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _fallNeedleCollider = obj;
        _transform = transform;
        _rb = rb;
        _sounds = sounds;
        _boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    public void Enter()
    {
        _tokenSource = new CancellationTokenSource();
        _rb.useGravity = false;
        _boxCollider.enabled = false;
        if (_sounds) _sounds.PlayEnemySE(EnemySeEnum.Voice_Laughter);
        Attack().Forget();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        _boxCollider.enabled = true;
        _rb.useGravity = true;
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
    }
    
    private async UniTask Attack()
    {
        _transform.position = new Vector3(_transform.position.x, _transform.position.y + _height);
        _animator.SetTrigger(_fallNeedle);
        await UniTask.WaitUntil(() => _animator && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"), cancellationToken : _tokenSource.Token);
        _fallNeedleCollider.SetActive(true);
        await UniTask.WaitUntil(() => _fallNeedleCollider.activeSelf == false, cancellationToken : _tokenSource.Token);
        _enemyBase.ChangeState(_freezeState);
    }
}
