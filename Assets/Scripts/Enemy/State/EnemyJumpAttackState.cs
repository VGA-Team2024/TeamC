using UnityEngine;

/// <summary> 敵のジャンプ攻撃ステート </summary>
public class EnemyJumpAttackState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _jump = Animator.StringToHash("Jump");
    private readonly Transform _transform;
    private readonly AnimationCurve _animationCurve;
    private float _curveRate;
    private float _speed;
    private Vector2 _startPos;
    private float _x;
    private readonly float _duration;
    private Vector2 _playerPos;

    public EnemyJumpAttackState(EnemyBase enemyBase, EnemyFreezeState freezeState, Animator animator, Transform transform, AnimationCurve animationCurve)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _transform = transform;
        _animationCurve = animationCurve;
        _duration = _animationCurve.keys[_animationCurve.length - 1].time;
    }
    
    public void Enter()
    {
        if (_animator) _animator.SetTrigger(_jump);
        _startPos = _transform.position;
        _x = 0f;
        _speed = 1f;
        _transform.eulerAngles = new Vector2(0, _playerPos.x > _transform.position.x ? 0 : 180);
    }

    public void Execute()
    {
        Attack();
    }

    public void Exit()
    {
        
    }
    
    private void Attack()
    {
        _curveRate = Mathf.Clamp(_curveRate + _speed * Time.deltaTime, 0f, 1f);
        _x += Time.deltaTime;
        var y = _animationCurve.Evaluate(_curveRate);
        _transform.position = _playerPos.x > _transform.position.x ? 
            new Vector2(_startPos.x + _x, _startPos.y + y) : new Vector2(_startPos.x - _x, _startPos.y + y);
        if(Mathf.Approximately(_curveRate, 1f)){
            _speed = -1;
        }

        if (_x >= _duration)
        {
            _enemyBase.ChangeState(_freezeState);
        }
    }
    
    public void GetPlayerPos(Vector2 pos)
    {
        _playerPos = pos;
    }
}
