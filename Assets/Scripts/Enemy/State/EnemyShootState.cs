using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyShootState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _shoot = Animator.StringToHash("Shoot");
    private readonly Transform _transform;
    private Vector2 _playerPos;
    private readonly GameObject _obj;
    private readonly Vector2 _offSet;
    
    public EnemyShootState(EnemyBase enemyBase, EnemyFreezeState freezeState, Animator animator, Transform transform,Vector2 offSet, GameObject obj)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _transform = transform;
        _offSet = offSet;
        _obj = obj;
    }
    
    public void Enter()
    {
        if (_animator) _animator.SetTrigger(_shoot);
        Shoot().Forget();
        var obj = ObjectInstantiator.InstantiateObject(_obj, GetOffSet(), Quaternion.identity);
        if (obj.TryGetComponent(out IPlayerTarget pt)) { pt.GetPlayerPos(_playerPos); }
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }

    private async UniTask Shoot()
    {
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        _enemyBase.ChangeState(_freezeState);
    }

    public void GetPlayerPos(Vector2 playerPos)
    {
        _playerPos = playerPos;
    }

    private Vector2 GetOffSet()
    {
        return _transform.position + _transform.right * _offSet.x + _transform.up * _offSet.y;
    }
}

public static class ObjectInstantiator
{
    public static T InstantiateObject<T>(T obj, Vector3 pos, Quaternion rot) where T : Object
    {
        return Object.Instantiate(obj, pos, rot);
    }
}
