using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour,ITeleportable
{
    private Rigidbody _rb;
    private PlayerControls _controls;
    private SpriteRenderer _sr;

    private Vector2 _dir; //ActionMapのMoveの値を保存するVector2
    [SerializeField, InspectorVariantName("プレイヤーの移動速度")] private float _moveSpeed = 2;
    [SerializeField, InspectorVariantName("プレイヤーのジャンプ力")] private float _jumpPower = 5;
    [SerializeField, InspectorVariantName("ジャンプを長押しできる時間")] private float _jumpTime = 0.5f;
    [SerializeField, InspectorVariantName("着地判定用Rayの長さ")] private float _rayLength = 0.55f;
    [SerializeField, InspectorVariantName("左右入力で反転するゲームオブジェクト")]private GameObject _flipObject;
    
    
    private bool _dirRight = true;  //プレイヤーがどちら側を向いているか
    public bool PlayerFlip
    {
        get => _dirRight;
        private set
        {
            if(_sr)
                _sr.flipX = value;
            else
                _flipObject.transform.rotation = Quaternion.Euler(0,(value ? 0 : 180),0);
            _dirRight = value;
        }
    }
    private bool _isMove = true;
    public bool IsMove { set { _isMove = value; } }

    private bool _isFreeze;
    public bool IsFreeze { set { _isFreeze = value; } }

    private CancellationTokenSource _tokenSource;
    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if(!_flipObject)
            _sr = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        //InputSystemで作ったPlayerControlsのインスタンスを生成
        _controls = new PlayerControls();
        _controls.InGame.Jump.started += OnJump;
        _controls.InGame.Jump.canceled += JumpCancel;
        _controls.InGame.Move.started += OnMove;  //入力はじめ
        _controls.InGame.Move.performed += OnMove;//値が変わった時
        _controls.InGame.Move.canceled += OnMove; //入力終わり
    }

    private void OnDestroy()
    {
        _controls.Dispose();
        _controls.InGame.Jump.started -= OnJump;
        _controls.InGame.Jump.canceled -= JumpCancel;
        _controls.InGame.Move.started -= OnMove;  //入力はじめ
        _controls.InGame.Move.performed -= OnMove;//値が変わった時
        _controls.InGame.Move.canceled -= OnMove; //入力終わり
        //_controls.InGame.Attack.started -= OnAttack;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private async void OnJump(InputAction.CallbackContext context)
    {
        _tokenSource = new();
        // 地面についているかの判定
        if (Physics.Raycast(transform.position, Vector3.down  ,out RaycastHit hitInfo, _rayLength))
        {
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _rb.useGravity = false;
                _rb.AddForce(Vector2.up * _jumpPower, ForceMode.Impulse);
                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_jumpTime), cancellationToken: _tokenSource.Token);
                    _rb.useGravity = true;
                }
                catch (OperationCanceledException e)
                {
                    _rb.useGravity = true;
                }
            }
        }
    }
    private void JumpCancel(InputAction.CallbackContext context)
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
    }


    // ActionMapのMove
    private void OnMove(InputAction.CallbackContext context)
    {
        _dir = context.ReadValue<Vector2>();
        //今向いている方向と逆に向いたときに攻撃判定用ゲームオブジェクトの位置を変える
        if ((_dir.x > 0 && !PlayerFlip) || (_dir.x < 0 && PlayerFlip))
        {
            PlayerFlip = !PlayerFlip;
        }
    }

    private void FixedUpdate()
    {
        //ジャンプ判定用Rayの表示
        Debug.DrawRay(transform.position, Vector3.down * _rayLength, Color.black);

        // 移動操作可能
        if (_isMove)
        {
            //左右移動
            _rb.velocity = new Vector3(_dir.x * _moveSpeed, _rb.velocity.y, 0);
        }

        // 完全固定
        if(_isFreeze)
        {
            _rb.velocity = Vector3.zero;
        }
    }

    public void Teleport(Vector3 position)
    {
        this.transform.position = position;
    }
}
