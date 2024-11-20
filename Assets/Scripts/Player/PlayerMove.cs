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
    private Player _player;
    private readonly int MoveHorizontal = Animator.StringToHash("MoveHorizontal");
    private readonly int IsGround = Animator.StringToHash("IsGround");

    private Vector2 _dir; //ActionMapのMoveの値を保存するVector2
    [SerializeField, InspectorVariantName("プレイヤーの移動速度")] private float _moveSpeed = 2;
    [SerializeField, InspectorVariantName("プレイヤーのジャンプ力")] private float _jumpPower = 5;
    [SerializeField, InspectorVariantName("ジャンプを長押しできる時間")] private float _jumpTime = 0.5f;
    [SerializeField, InspectorVariantName("ジャンプ長押し中の重力")] private float _jumpPressGravity = 5;
    [SerializeField, InspectorVariantName("重力")]float _gravityScale = 20;
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
    private bool _isGround; //設置判定
    
    private bool _isMove = true; //移動不可状態の判定
    public bool IsMove { set { _isMove = value; } }

    private bool _isFreeze; //　完全固定状態の判定
    public bool IsFreeze { set { _isFreeze = value; } }

    //重力を使うかの判定
    bool _useGravity = true;
    
    private CancellationTokenSource _tokenSource;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if(!_flipObject)
            _sr = GetComponent<SpriteRenderer>();
        _player = GetComponent<Player>();
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
        if (_isGround)
        {
            _useGravity = false;
            _rb.AddForce(Vector2.up * _jumpPower, ForceMode.Impulse);
            _player.PlayerSounds.PlayerSEPlay(PlayerSoundEnum.Jump);
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_jumpTime), cancellationToken: _tokenSource.Token);
                _useGravity = true;
            }
            catch (OperationCanceledException e)
            {
                _useGravity = true;
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
        Debug.DrawRay(transform.position, Vector3.down * _rayLength, Color.yellow);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, _rayLength))
        {
            if (!_isGround)
            {

                _player.PlayerSounds.PlayerSEPlay(PlayerSoundEnum.JumpLandhing);
            }
            _isGround = hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground");
        }
        else
        {
            _isGround = false;
        }
        // 移動操作可能
        if (_isMove)
        {
            //左右移動
            _rb.velocity = new Vector3(_dir.x * _moveSpeed, _rb.velocity.y, 0);
        }

        //重力
        if (_useGravity)
        {
            _rb.AddForce(new Vector3(0, _gravityScale * -1, 0), ForceMode.Acceleration);
        }
        else
        {
            _rb.AddForce(new Vector3(0, _jumpPressGravity * -1, 0), ForceMode.Acceleration);
        }
        
        // 完全固定
        if(_isFreeze)
        {
            _rb.velocity = Vector3.zero;
        }
        _player.Animator.SetFloat(MoveHorizontal,Mathf.Abs(_rb.velocity.x));
        _player.Animator.SetBool(IsGround,_isGround);
    }

    public void Teleport(Vector3 position)
    {
        position.z = 0;
        this.transform.position = position;
    }
}
