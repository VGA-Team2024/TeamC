using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> 敵の突進ステート </summary>
public class EnemyRushState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _rush = Animator.StringToHash("Rush");
    private readonly Transform _transform;
    private readonly float _distance;
    private readonly float _speed;
    private readonly EnemySounds _sounds;
    private float _destination;
    private bool _canRush;
    private CancellationTokenSource _tokenSource;
    
    private readonly float _rayLength;
    private readonly Vector3 _rayOffset = new Vector3(0, -0.5f, 0);
    private readonly Vector2 _rightRayDir;
    private readonly Vector2 _leftRayDir;

    public EnemyRushState(EnemyBase enemyBase, EnemyFreezeState freezeState, Animator animator,Transform transform, float dis, float speed, EnemySounds sounds = null)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _transform = transform;
        _distance = dis;
        _speed = speed;
        _sounds = sounds;
        
        var colliderSize = transform.gameObject.GetComponent<BoxCollider>().size;
        _rayLength = colliderSize.y / 2 + 1f;
        _rightRayDir = new Vector2(colliderSize.x, -colliderSize.y).normalized;
        _leftRayDir = new Vector2(-colliderSize.x, -colliderSize.y).normalized;
    }
    
    public void Enter()
    {
        _destination = _transform.position.x + _transform.right.x * _distance;
        _tokenSource = new CancellationTokenSource();
        Set().Forget();
    }

    public void Execute()
    {
        if (_canRush) Cansel();
        if (_canRush) Rush(); // Cancelで_canRushがfalseになってなかったら入るため分けた
        
        float direction = Mathf.Sign(_destination - _transform.position.x);
        float currentDirection = Mathf.Sign(_transform.right.x);
        
        if (!Mathf.Approximately(direction, currentDirection))
        {
            _enemyBase.ChangeState(_freezeState);
        }
    }

    public void Exit()
    {
        if (_animator) _animator.SetBool(_rush, false);
        if (_sounds) _sounds.PlayEnemySE(EnemySeEnum.Brake);
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
    }
    
    private async UniTask Set()
    {
        _canRush = false;
        if (_animator) _animator.SetBool(_rush, true);
        if (_sounds) _sounds.PlayEnemySE(EnemySeEnum.Rush);
        await UniTask.WaitUntil(() => _animator && _animator.GetCurrentAnimatorStateInfo(0).IsName("Rush2"), cancellationToken : _tokenSource.Token);
        _canRush = true;
    }

    private void Rush()
    {
        Vector3 newPos = _transform.position + _transform.right * (Time.deltaTime * _speed);
        _transform.position = new Vector3(newPos.x, newPos.y, 0);
    }
    
    private void Cansel()
    {
        // 前に床がなければ止まる
        Vector3 rayOrigin = _transform.position + _rayOffset;
        bool hit = Physics.Raycast(rayOrigin, _transform.rotation.y > 0 ? _leftRayDir : _rightRayDir, out RaycastHit hitInfo, _rayLength);

        // 前が壁なら止まる
        bool wallHit = Physics.Raycast(rayOrigin, _transform.right, out RaycastHit wallHitInfo, _rayLength);
        
        if (!hit || LayerMask.LayerToName(hitInfo.transform.gameObject.layer) != "Ground" || wallHit && LayerMask.LayerToName(wallHitInfo.transform.gameObject.layer) == "Ground")
        {
            _canRush = false;
            _enemyBase.ChangeState(_freezeState);
        }
    }
}