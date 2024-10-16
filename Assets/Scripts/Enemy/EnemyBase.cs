using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float _speed;
    protected IEnemyState _currentState = default;
    private EnemyIdleState _idleState = default;
    protected EnemyHp _hp = default;
    protected bool _isDeath = false;

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

    protected void ChangeState(IEnemyState nextState)
    {
        if (_currentState != null) _currentState.Exit();

        _currentState = nextState;
        _currentState.Enter();
    }
}