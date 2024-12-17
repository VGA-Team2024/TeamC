using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour, ITeleportable
{
    private Rigidbody _rb;
    private Player _player;
    private PlayerControls _controls;
    private readonly int MoveHorizontal = Animator.StringToHash("MoveHorizontal");
    private readonly int IsGround = Animator.StringToHash("IsGround");
    private readonly int MusicBox = Animator.StringToHash("MusicBox");
    private readonly int MoveVertical = Animator.StringToHash("MoveVertical");
    private readonly int DashingHash = Animator.StringToHash("Dashing");
    private readonly int JumpStart = Animator.StringToHash("JumpStart");
    private readonly int DirRight = Animator.StringToHash("DirRight");

    public Vector2 Dir {get; private set; }
    [Header("通常移動")]
    [SerializeField, InspectorVariantName("移動速度")] private float _moveSpeed = 20;
    [Header("ダッシュ")]
    [SerializeField, InspectorVariantName("ダッシュ時間")] private float _dashTime = 0.2f;
    [SerializeField, InspectorVariantName("ダッシュ速度")] private float _dashSpeed = 60;
    [SerializeField, InspectorVariantName("ダッシュ中に縦にズレる高さ")] private float _dashFloatHeight = 0.1f;
    [SerializeField, InspectorVariantName("クールタイム")] private float _dashCoolTime = 1.5f;
    [Header("ジャンプ")]
    [SerializeField, InspectorVariantName("ジャンプ力(1回目)")] private float _firstJumpPower = 24;
    [SerializeField, InspectorVariantName("ジャンプ力(2回目)")] private float _secondJumpPower = 15;
    [SerializeField, InspectorVariantName("ジャンプを長押しできる時間(1回目)")] private float _firstJumpTime = 0.5f;
    [SerializeField, InspectorVariantName("ジャンプを長押しできる時間(2回目)")] private float _secondJumpTime = 0.5f;
    [SerializeField, InspectorVariantName("ジャンプ長押し中の重力")] private float _jumpPressGravity = 5;
    [Header("重力")]
    [SerializeField, InspectorVariantName("重力")] private float _gravityScale = 20;
    [SerializeField, InspectorVariantName("落下速度の上限")] private float _maxFallingSpeed = 50;
    [Header("プログラマー向け")]
    [SerializeField, InspectorVariantName("左右入力で反転するゲームオブジェクト")]private GameObject _flipObject;
    
    [SerializeField] private Vector3 _boxSize;
    [SerializeField] private float _dis;
    private RaycastHit _hit;
    
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
                PlayerEffectManager.Instance.ReStartPlayEffect(PlayEffectName.PlayerMoveEffect);   
            }
        }
    }
    private bool _isGround; //設置判定
    
    private bool _isMove = true; //移動不可状態の判定
    public bool IsMove { set => _isMove = value; }
    
    public bool Dashing { get; private set; }
    private bool _canDash = true;
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
        _player = GetComponent<Player>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _isGround ? Color.green : Color.red;
        Gizmos.DrawCube(this.transform.position + Vector3.down * _dis, _boxSize);
    }

    private void FixedUpdate()
    {
        //bool boxHit = Physics.BoxCast(this.transform.position, _boxSize, Vector3.down, out RaycastHit _hit, Quaternion.identity, _dis);
        if(Physics.BoxCast(this.transform.position, _boxSize / 2, Vector3.down, out RaycastHit _hit, Quaternion.identity, _dis) 
           && _hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!_isGround)
            {
                _player.PlayerSounds.PlayerSEPlay(PlayerSoundEnum.JumpLandhing);
            }
            _onAirJump = true;
            _isGround = true;
        }
        else
        {
            _isGround = false;
        }
        //今向いている方向と逆に向いたときに攻撃判定用ゲームオブジェクトの位置を変える
        if ((Dir.x > 0 && !PlayerFlip) || (Dir.x < 0 && PlayerFlip))
        {
            PlayerFlip = !PlayerFlip;
        }
        
        // 移動操作可能
        if (_isMove)
        {
            //左右移動
            _rb.velocity = new Vector3(math.sign(Dir.x) * _moveSpeed, _rb.velocity.y, 0);
        }
        
        Gravity();
        
        // 完全固定
        _player.Animator.SetFloat(MoveHorizontal,Mathf.Abs(_rb.velocity.x));
        _player.Animator.SetFloat(MoveVertical,_rb.velocity.y);
        _player.Animator.SetBool(IsGround,_isGround);
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
            PlayerEffectManager.Instance.InstanceEffect(InstancePlayEffectName.PlayerJumpEffect,
                new Vector3(gameObject.transform.transform.position.x,
                    gameObject.transform.transform.position.y - 2,
                    gameObject.transform.transform.position.z)
                , new Vector3(95, 0, 0)
            );
            if(!_player.PlayerStatus.IsSecondJumpRelease)
                return; // 空中ジャンプが解放されていないならreturnする
            jumpPower = _secondJumpPower;
            jumpTime = _secondJumpTime;
            _onAirJump = false;
        }

        PlayerEffectManager.Instance.StopPlayEffect(PlayEffectName.PlayerMoveEffect);
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
        Dir = context.ReadValue<Vector2>();
    }

    private async void OnDash(InputAction.CallbackContext context)
    {
        if(!_player.PlayerStatus.IsDashRelease || !_canDash)
            return; //ダッシュが解放されていない or ダッシュ不可
        if (_isMove)
        {
            PlayerEffectManager.Instance.PlayEffect(PlayEffectName.PlayerDashEffect,
                transform.GetChild(0).localEulerAngles.y);
            if (_jumpCancelToken != null)
            {
                _jumpCancelToken.Cancel();
                _jumpCancelToken.Dispose();
                _jumpCancelToken = null;
            }
            _jumpCancelToken = new CancellationTokenSource();

            Dashing = true;
            _canDash = false;
            _player.Animator.SetBool(DashingHash,true);
            IsFreeze = (true, true);
            transform.position += Vector3.up * _dashFloatHeight; 
            _rb.velocity = new Vector3((_dirRight ? 1 : -1) * _dashSpeed, 0, 0);
            await UniTask.Delay(TimeSpan.FromSeconds(_dashTime), cancellationToken:_player.CancellationToken);
            _player.Animator.SetBool(DashingHash,false);
            IsFreeze = (false, false);
            Dashing = false;
            
            //クールタイム処理
            await UniTask.Delay(TimeSpan.FromSeconds(_dashCoolTime - _dashTime), cancellationToken: _player.CancellationToken);
            _canDash = true;
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