using Cysharp.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEngine;

public class EnemyFlyState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _fly = Animator.StringToHash("Fly");
    private readonly Transform _transform;
    private readonly float _height;
    private const float Speed = 3f;
    private float _destination;

    public EnemyFlyState(EnemyBase enemyBase, EnemyFreezeState freezeState, Animator animator, Transform transform, float height)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _transform = transform;
        _height = height;
    }
    
    public void Enter()
    {
        _animator.SetBool(_fly, true);
        _destination = _transform.position.y + _height;
        Fly().Forget();
    }

    public void Execute()
    {
        _transform.Translate(Vector3.up * (Time.deltaTime * Speed));
    }

    public void Exit()
    {
        
    }
    
    private async UniTask Fly()
    {
        await UniTask.WaitUntil(() => _transform.position.y >= _destination);
        _enemyBase.ChangeState(_freezeState);
    }
}
