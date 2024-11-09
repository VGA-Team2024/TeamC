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
    }

    public void Execute()
    {
        Attack();
        
        if (_x >= _duration)
        {
            _enemyBase.ChangeState(_freezeState);
        }
    }

    public void Exit()
    {
        
    }
    
    private void Attack()
    {
        _curveRate = Mathf.Clamp(_curveRate + _speed * Time.deltaTime, 0f, 1f);
        _x += Time.deltaTime;
        var y = _animationCurve.Evaluate(_curveRate);
        _transform.position = (Vector3)_startPos + _transform.rotation * new Vector2(_x, y);
        
        if(Mathf.Approximately(_curveRate, 1f)){
            _speed = -1;
        }
    }
}
