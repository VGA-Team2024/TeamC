using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary> ある範囲に入ったらplayerを追いかける敵 </summary>
public class ChaseEnemy : EnemyBase, IPlayerTarget
{
    [SerializeField, Header("空中か")] private bool _isFly;
    [SerializeField, Header("巡回する範囲")] private float _patrolArea;
    [SerializeField, Header("Playerにつけるタグの名前")] private string _playerTag;
    private SpriteRenderer _spriteRenderer = default;
    private CancellationToken _token = default;
    private bool _isStop = false;
    
    private EnemyWalkState _walkState = default;
    private EnemyChaseState _chaseState = default;
    private EnemyDeathState _deathState = default;
    
    protected override void OnStart()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerTransform = null;
        _token = this.GetCancellationTokenOnDestroy();
        
        _walkState = new EnemyWalkState(this, transform, _speed, _patrolArea, transform.position, _spriteRenderer);
        _chaseState = new EnemyChaseState(this, transform, _speed, _spriteRenderer, _isFly);
        _deathState = new EnemyDeathState(this);
    }

    protected override void OnUpdate()
    {
        if (_playerTransform)
        {
            if (_isStop) return;
            if (_currentState != _chaseState)
            {
                ChangeState(_chaseState);
                return;
            }
            _chaseState.GetPlayerPos(_playerTransform.position);
        }
        else
        {
            if (_currentState != _walkState) ChangeState(_walkState);
        }

        if (_hp.CurrentHp <= 0)
        {
            if (_isDeath) return;
            
            _isDeath = true;
            ChangeState(_deathState);
        }
    }
    
    public void GetPlayerMove(PlayerMove playerMove)
    {
        _playerTransform = playerMove.gameObject.transform;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isStop) return;
        // todo:playerへのダメージはここ
        _isStop = true;
        ChangeState(_idleState);
        StartFreezeTimer().Forget();
    }

    private async UniTask StartFreezeTimer()
    {
        await UniTask.Delay(1000, cancellationToken:_token);
        _isStop = false;
    }
}
