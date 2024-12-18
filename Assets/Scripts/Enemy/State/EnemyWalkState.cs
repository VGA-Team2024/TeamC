using UnityEngine;

/// <summary> 敵の巡回ステート </summary>
public class EnemyWalkState : IEnemyState
{
    private readonly Animator _animator;
    private readonly int _walk = Animator.StringToHash("Walk");
    private readonly Transform _transform;
    private readonly float _speed;
    private readonly float _patrolArea;
    private readonly Vector2 _startPos;
    
    private readonly float _rayLength;
    private readonly Vector3 _rayOffset = new Vector3(0, -0.5f, 0);
    private readonly Vector2 _rightRayDir;
    private readonly Vector2 _leftRayDir;

    private readonly Vector2 right = new Vector2(0, 180);
    private readonly Vector2 left = new Vector2(0, 0);
    
    public EnemyWalkState(Animator animator, Transform transform, float speed, float area)
    {
        _animator = animator;
        _transform = transform;
        _startPos = transform.position;
        _speed = speed;
        _patrolArea = area;
        var colliderSize = transform.gameObject.GetComponent<BoxCollider>().size;
        _rayLength = colliderSize.y / 2 + 1f;
        _rightRayDir = new Vector2(colliderSize.x, -colliderSize.y).normalized;
        _leftRayDir = new Vector2(-colliderSize.x, -colliderSize.y).normalized;
    }
    
    
    public void Enter()
    {
        _animator.SetBool(_walk, true);
    }

    public void Execute()
    {
        Direction();
        Walk();
    }

    public void Exit()
    {
        _animator.SetBool(_walk, false);
    }
    
    private void Walk()
    {
        _transform.Translate(Vector3.right * -(Time.deltaTime * _speed));
    }

    private void Direction()
    {
        // 前に床がなければ引き返す
        Vector3 rayOrigin = _transform.position + _rayOffset;
        bool hit = Physics.Raycast(rayOrigin, _transform.rotation.y > 0 ? _rightRayDir : _leftRayDir, out RaycastHit hitInfo, _rayLength);

        // 前が壁なら引き返す
        bool wallHit = Physics.Raycast(rayOrigin, -_transform.right, out RaycastHit wallHitInfo, _rayLength);
        
        if (!hit || LayerMask.LayerToName(hitInfo.transform.gameObject.layer) != "Ground" || wallHit && LayerMask.LayerToName(wallHitInfo.transform.gameObject.layer) == "Ground")
        {
            _transform.eulerAngles = new Vector2(0, _transform.eulerAngles.y == 0 ? right.y : left.y);
        }
        
        if (_transform.position.x <= _startPos.x + 0.01f)
        {
            _transform.eulerAngles = right;
        }

        if (_transform.position.x >= _startPos.x + _patrolArea - 0.01f)
        {
            _transform.eulerAngles = left;
        }
    }
}
