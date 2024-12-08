using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour, ITeleportable
{
    private Rigidbody _rb;
    private SpriteRenderer _sr;
    private Player _player;
    private PlayerControls _controls;
    private readonly int MoveHorizontal = Animator.StringToHash("MoveHorizontal");
    private readonly int IsGround = Animator.StringToHash("IsGround");
    private readonly int MusicBox = Animator.StringToHash("MusicBox");
    private readonly int MoveVertical = Animator.StringToHash("MoveVertical");
    private readonly int DashingHash = Animator.StringToHash("Dashing");
    private readonly int JumpStart = Animator.StringToHash("JumpStart");
    private readonly int DirRight = Animator.StringToHash("DirRight");

    private Vector2 _dir; //ActionMapのMoveの値を保存するVector2
    [SerializeField, InspectorVariantName("プレイヤーの移動速度")] private float _moveSpeed = 20;
    [SerializeField, InspectorVariantName("プレイヤーダッシュ時間")] private float _dashTime = 0.2f;
    [SerializeField, InspectorVariantName("プレイヤーダッシュ速度")] private float _dashSpeed = 60;
    [SerializeField, InspectorVariantName("プレイヤーのジャンプ力(1回目)")] private float _firstJumpPower = 24;
    [SerializeField, InspectorVariantName("プレイヤーのジャンプ力(2回目)")] private float _secondJumpPower = 15;
    [SerializeField, InspectorVariantName("ジャンプを長押しできる時間(1回目)")] private float _firstJumpTime = 0.5f;
    [SerializeField, InspectorVariantName("ジャンプを長押しできる時間(2回目)")] private float _secondJumpTime = 0.5f;
    [SerializeField, InspectorVariantName("ジャンプ長押し中の重力")] private float _jumpPressGravity = 5;
    [SerializeField, InspectorVariantName("重力")] private float _gravityScale = 20;
    [SerializeField, InspectorVariantName("落下速度の上限")] private float _maxFallingSpeed = 50;
    [SerializeField, InspectorVariantName("左右入力で反転するゲームオブジェクト")]private GameObject _flipObject;
    private bool _dirRight = true;  //プレイヤーがどちら側を向いているか
    public bool PlayerFlip
    {
        get => _dirRight;
        private set
        {
            if(!_isMove) return;
            // 見た目の反転はアニメーターコントローラーがする
            // FlipShaftは残しておく
            _flipObject.transform.rotation = Quaternion.Euler(0,(value ? 0 : 180),0);
            
            _dirRight = value;
            _player.Animator.SetBool(DirRight, value);
            if (_isGround)
            {
                EffectManager.Instance.ReStartPlayEffect(PlayEffectName.PlayerDashEffect);   
            }
        }
    }
    private bool _isGround; //設置判定
    
    private bool _isMove = true; //移動不可状態の判定
    public bool IsMove { set => _isMove = value; }
    
    public bool Dashing { get; private set; }
    private bool _onAirJump;
    //　移動量、重力無効化
    public (bool value, bool VelocityZero) IsFreeze {
        set
        {
            if(value.VelocityZero) _rb.velocity = Vector3.zero;
            _gravityEnum = value.value ? GravityEnum.NoGravity : GravityEnum.JumpDown;
            _isMove = !value.value; 
        }
    }
    
    //重力状態のEnum
    GravityEnum _gravityEnum = GravityEnum.JumpDown;

    private CancellationTokenSource _jumpCancelToken = new CancellationTokenSource();
    
    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.InGame.Jump.started += OnJump;
        _controls.InGame.Jump.canceled += JumpCancel;
        _controls.InGame.Move.started += OnMove;  //入力はじめ
        _controls.InGame.Move.performed += OnMove;//値が変わった時
        _controls.InGame.Move.canceled += OnMove; //入力終わり
        _controls.InGame.Dash.started += OnDash;
    }

    private void OnDestroy()
    {
        _controls.Dispose();
        _controls.InGame.Jump.started -= OnJump;
        _controls.InGame.Jump.canceled -= JumpCancel;
        _controls.InGame.Move.started -= OnMove;  //入力はじめ
        _controls.InGame.Move.performed -= OnMove;//値が変わった時
        _controls.InGame.Move.canceled -= OnMove; //入力終わり
        _controls.InGame.Dash.started -= OnDash;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if(!_flipObject)
            _sr = GetComponent<SpriteRenderer>();
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        //今向いている方向と逆に向いたときに攻撃判定用ゲームオブジェクトの位置を変える
        if ((_dir.x > 0 && !PlayerFlip) || (_dir.x < 0 && PlayerFlip))
        {
            PlayerFlip = !PlayerFlip;
        }
        
        // 移動操作可能
        if (_isMove)
        {
            //左右移動
            _rb.velocity = new Vector3(_dir.x * _moveSpeed, _rb.velocity.y, 0);
        }
        
        Gravity();
        
        // 完全固定
        _player.Animator.SetFloat(MoveHorizontal,Mathf.Abs(_rb.velocity.x));
        _player.Animator.SetFloat(MoveVertical,_rb.velocity.y);
        _player.Animator.SetBool(IsGround,_isGround);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!_isGround)
            {
                _player.PlayerSounds.PlayerSEPlay(PlayerSoundEnum.JumpLandhing);
                _onAirJump = true;
            }

            _isGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGround = false;
        }
    }

    private async void OnJump(InputAction.CallbackContext context)
    {
        
        if (!_isMove || !_onAirJump) return;
        float jumpPower;
        float jumpTime;
        if (_isGround)
        {// 地上のジャンプなら
            jumpPower = _firstJumpPower;
            jumpTime = _firstJumpTime;
        }
        else
        {// 空中のジャンプなら
            if(!_player.PlayerStatus.IsSecondJumpRelease)
                return; // 空中ジャンプが解放されていないならreturnする
            jumpPower = _secondJumpPower;
            jumpTime = _secondJumpTime;
            _onAirJump = false;
        }
        _jumpCancelToken = new();
        _gravityEnum = GravityEnum.JumpUp;
        _rb.velocity = new Vector3(_rb.velocity.x, 0,0);
        _rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        _player.PlayerSounds.PlayerSEPlay(PlayerSoundEnum.Jump);
        _player.Animator.SetTrigger(JumpStart);
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(jumpTime), cancellationToken: _jumpCancelToken.Token);
            _gravityEnum = GravityEnum.JumpDown;
        }
        catch (OperationCanceledException e)
        {
            
        }
    }
    private void JumpCancel(InputAction.CallbackContext context)
    {
        if (_jumpCancelToken != null)
        {
            _jumpCancelToken.Cancel();
            _jumpCancelToken.Dispose();
            _jumpCancelToken = null;
        }
        if(_gravityEnum == GravityEnum.JumpUp)
            _gravityEnum = GravityEnum.JumpDown;
    }

    // ActionMapのMove
    private void OnMove(InputAction.CallbackContext context)
    {
        if (_isGround)
        {
            EffectManager.Instance.PlayEffect(PlayEffectName.PlayerDashEffect, 0);   
        }
        _dir = context.ReadValue<Vector2>();
    }

    private async void OnDash(InputAction.CallbackContext context)
    {
        if(!_player.PlayerStatus.IsDashRelease)
            return; //ダッシュが解放されていないならreturnする
        if (_isMove)
        {
            EffectManager.Instance.PlayEffect(PlayEffectName.PlayerBlinkEffect,
                transform.GetChild(0).localEulerAngles.y);
            if (_jumpCancelToken != null)
            {
                _jumpCancelToken.Cancel();
                _jumpCancelToken.Dispose();
                _jumpCancelToken = null;
            }

            Dashing = true;
            _player.Animator.SetBool(DashingHash,true);
            _jumpCancelToken = new CancellationTokenSource();
            IsFreeze = (true, true);
            _rb.velocity = new Vector3((_dirRight ? 1 : -1) * _dashSpeed, 0, 0);
            await UniTask.Delay(TimeSpan.FromSeconds(_dashTime));
            _player.Animator.SetBool(DashingHash,false);
            IsFreeze = (false, false);
            Dashing = false;
        }
    }
    
    void Gravity()
    {
        //重力
        if (_gravityEnum == GravityEnum.JumpDown)
        {
            _rb.AddForce(new Vector3(0, _gravityScale * -1, 0), ForceMode.Acceleration);
        }
        else if(_gravityEnum == GravityEnum.JumpUp)
        {
            _rb.AddForce(new Vector3(0, _jumpPressGravity * -1, 0), ForceMode.Acceleration);
        }
        else
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0, 0);
        }
        
        //落下速度に上限を設定
        if (_rb.velocity.y < _maxFallingSpeed * -1)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _maxFallingSpeed * -1, _rb.velocity.z);
        }
    }
    
    public void Teleport(Vector3 position)
    {
        position.z = 0;
        this.transform.position = position;
    }

    private enum GravityEnum
    {
        JumpUp,
        JumpDown,
        NoGravity,
    }
}