using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary> 敵が死んだときのステート </summary>
public class EnemyDeathState : IEnemyState
{
    private static event Action<EnemyBase> OnEnemyDestroyed;
    private readonly EnemyBase _enemyBase;
    private readonly ParticleSystem _particle;
    private readonly GameObject _obj;
    private GameObject _generateObj;
    
    /// <param name="generateObj"> 死んだときに生成する敵などがいたら入れる </param>
    public EnemyDeathState(EnemyBase enemyBase,ParticleSystem particle, GameObject obj, GameObject generateObj = null)
    {
        _enemyBase = enemyBase;
        _particle = particle;
        _obj = obj;
        _generateObj = generateObj;
    }
    
    public void Enter()
    {
        _obj.transform.rotation = Quaternion.Euler(0, 0, 180); 
        OnEnemyDestroyed?.Invoke(_enemyBase);
        _particle.Play();
        Destroy().Forget();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }

    private async UniTask Destroy()
    {
        await UniTask.WaitUntil(() => _particle.isStopped);
        GameObject.Destroy(_obj);
    }
    
    public static void SubscribeToDestroyedEvent(Action<EnemyBase> listener)
    {
        OnEnemyDestroyed += listener; 
    }

    public static void UnsubscribeFromDestroyedEvent(Action<EnemyBase> listener)
    {
        OnEnemyDestroyed -= listener;
    }
}
