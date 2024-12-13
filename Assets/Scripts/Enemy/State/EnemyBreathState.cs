using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> ドラゴンのブレス攻撃 </summary>
public class EnemyBreathState : IEnemyState
{
    private readonly EnemyBase _enemyBase;
    private readonly EnemyFreezeState _freezeState;
    private readonly Animator _animator;
    private readonly int _breath = Animator.StringToHash("Breath");
    private readonly Transform _transform;
    private readonly GameObject _breathObj;
    private readonly GameObject _breathObjs;
    private Vector2 _playerPos;
    private bool _isFly;
    
    public EnemyBreathState(EnemyBase enemyBase, EnemyFreezeState freezeState, Animator animator, Transform transform, GameObject breathObj, GameObject breathObjs)
    {
        _enemyBase = enemyBase;
        _freezeState = freezeState;
        _animator = animator;
        _transform = transform;
        _breathObj = breathObj;
        _breathObjs = breathObjs;
    }

    public void Enter()
    {
        if (_isFly) _breathObjs.SetActive(true);
        else ObjectInstantiator.InstantiateObject(_breathObj, (Vector2)_transform.position, Direction());
        
        PlayAnim().Forget();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }

    public void IsFly(bool fly)
    {
        _isFly = fly;
    }
    
    public void GetPlayerPos(Vector2 pos)
    {
        _playerPos = pos;
    }

    private Quaternion Direction()
    {
        var dir = _playerPos - (Vector2)_transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, angle);
    }
    
    private async UniTask PlayAnim()
    {
        if (_animator) _animator.SetTrigger(_breath);
        await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        _enemyBase.ChangeState(_freezeState);
    }
}
