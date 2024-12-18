using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyFlyState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyBreathState _breathState;
    private readonly Animator _animator;
    private readonly int _fly = Animator.StringToHash("Fly");
    private readonly Transform _transform;
    private readonly float _height;
    private readonly Rigidbody _rb;
    private const float Speed = 3f;
    private float _destination;

    public EnemyFlyState(EnemyBase enemyBase, EnemyBreathState breathState, Animator animator, Transform transform, Rigidbody rb, float height)
    {
        _enemyBase = enemyBase;
        _breathState = breathState;
        _animator = animator;
        _transform = transform;
        _rb = rb;
        _height = height;
    }
    
    public void Enter()
    {
        _animator.SetBool(_fly, true);
        _destination = _transform.position.y + _height;
        _rb.useGravity = false;
        Fly().Forget();
    }

    public void Execute()
    {
        _transform.Translate(Vector3.up * (Time.deltaTime * Speed));
    }

    public void Exit()
    {
        _rb.useGravity = true;
        _animator.SetBool(_fly, false);
    }
    
    private async UniTask Fly()
    {
        await UniTask.WaitUntil(() => _transform.position.y >= _destination);
        _enemyBase.ChangeState(_breathState);
    }
}
