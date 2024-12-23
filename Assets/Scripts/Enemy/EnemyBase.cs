using System;
using Unity.Mathematics;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyBase : MonoBehaviour
{
    [SerializeField, Header("死亡時に出現するエフェクト")] private GameObject _hoverEffectPrefab;
    [SerializeField, Header("死亡エフェクトが出るまでの時間")] private float _effectInstanceTime = 0.5f;
    [SerializeField] protected float _speed;
    protected IEnemyState _currentState;
    protected EnemyIdleState _idleState;
    protected EnemyHp _hp;
    protected bool _isDeath = false;
    protected PlayerMove _playerMove = default;

    private void Start()
    {
        _idleState = new EnemyIdleState(this);
        _hp = GetComponent<EnemyHp>();
        ChangeState(_idleState);
        OnStart();
    }
    
    protected virtual void OnStart(){}

    private void Update()
    {
        _currentState.Execute();
        OnUpdate();
    }
    
    protected virtual void OnUpdate(){}

    public async void ChangeState(IEnemyState nextState)
    {
        if (_currentState != null) _currentState.Exit();
        
        _currentState = nextState;
        _currentState.Enter();
        if (_isDeath)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_effectInstanceTime));
            if (_hoverEffectPrefab)
            {
                Instantiate(_hoverEffectPrefab, transform.position, quaternion.identity);   
            }
        }
    }
}