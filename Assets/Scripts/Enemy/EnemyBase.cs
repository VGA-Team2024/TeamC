using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float _speed;
    protected IEnemyState _currentState;
    protected EnemyIdleState _idleState;
    protected EnemyHp _hp;
    protected bool _isDeath = false;
    protected Transform _playerTransform = default;

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

    public void ChangeState(IEnemyState nextState)
    {
        if (_currentState != null) _currentState.Exit();

        _currentState = nextState;
        _currentState.Enter();
    }
}