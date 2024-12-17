using System.Collections.Generic;
using UnityEngine;

/// <summary> Enemyを管理するクラス </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<EnemyBase> _enemies;
    
    private void Start()
    {
        _enemies.AddRange(FindObjectsOfType<EnemyBase>());
    }
    
    private void RemoveEnemy(EnemyBase enemy)
    {
        if (_enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);
        }
    }
    
    private void OnEnable()
    {
        EnemyDeathState.SubscribeToDestroyedEvent(RemoveEnemy);
    }

    private void OnDisable()
    {
        EnemyDeathState.UnsubscribeFromDestroyedEvent(RemoveEnemy);
    }
}
