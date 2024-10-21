using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class PlayerMove : MonoBehaviour ,IDamageable
{
    private Rigidbody _rb;
    private PlayerControls _controls;
    private Cinemachine.CinemachineImpulseSource _source;
    private SpriteRenderer _sr;

    private Vector2 _dir; //ActionMapのMoveの値を保存するVector2
    [SerializeField, Header("プレイヤーの移動速度")] private float _moveSpeed = 2;
    [SerializeField, Header("プレイヤーのジャンプ力")] private float _jumpPower = 5;
    [SerializeField, Header("攻撃時に出すゲームオブジェクト")] private GameObject _attackCollider;
    [SerializeField, Header("着地判定用Rayの長さ")] private float _rayLength = 0.55f;
    [SerializeField, Header("ダメージを受けた時に吹き飛ぶ力")] private float _knockbackPower = 20;

    //プレイヤーがどちら側を向いているか
    private bool _dirRight = true;
    private bool _playerFlip 
    {
        get => _dirRight;
        set 
        {
            _sr.flipX = value;
            _dirRight = value;
        }
    }
    private bool _isMove = true;


    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _source = GetComponent<Cinemachine.CinemachineImpulseSource>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        //InputSystemで作ったPlayerControlsのインスタンスを生成
        _controls = new PlayerControls();
        _controls.InGame.Jump.started += OnJump;
        _controls.InGame.Move.started += OnMove;  //入力はじめ
        _controls.InGame.Move.performed += OnMove;//値が変わった時
        _controls.InGame.Move.canceled += OnMove; //入力終わり
        _controls.InGame.Attack.started += OnAttack;
    }

    private void OnDestroy()
    {
        _controls.Dispose();
        _controls.InGame.Jump.started -= OnJump;
        _controls.InGame.Move.started -= OnMove;  //入力はじめ
        _controls.InGame.Move.performed -= OnMove;//値が変わった時
        _controls.InGame.Move.canceled -= OnMove; //入力終わり
        _controls.InGame.Attack.started -= OnAttack;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        // 地面についているかの判定
        // タグの判定などをしていないので敵の上に居るときでもジャンプが可能になっている
        if (Physics.Raycast(transform.position, Vector3.down, _rayLength))
        {
            _rb.AddForce(Vector2.up * _jumpPower, ForceMode.Impulse);
        }
    }


    private void OnAttack(InputAction.CallbackContext context)
    {
        // 攻撃用当たり判定をアクティブにする
        _attackCollider.SetActive(true);
        // 非アクティブは_attackCollider自身がする
    }

    // ActionMapのMove
    private void OnMove(InputAction.CallbackContext context)
    {
        _dir = context.ReadValue<Vector2>();
        //今向いている方向と逆に向いたときに攻撃判定用ゲームオブジェクトの位置を変える
        if((_dir.x > 0 && _attackCollider.transform.localPosition.x < 0)
            ||(_dir.x < 0 && _attackCollider.transform.localPosition.x > 0))
        {
            Vector3 acp = _attackCollider.transform.localPosition;
            _attackCollider.transform.localPosition = new Vector3(acp.x * - 1, acp.y, acp.z);
            _playerFlip = !_playerFlip;
        }
    }

    private void FixedUpdate()
    {
        //ジャンプ判定用Rayの表示
        Debug.DrawRay(transform.position, Vector3.down * _rayLength,Color.black);
        if (_isMove)
        {
            //左右移動
            _rb.velocity = new Vector3(_dir.x * _moveSpeed, _rb.velocity.y, 0);
        }
    }

    // 攻撃されたときに呼ばれる
    public async void TakeDamage(int damage)
    {
        Debug.Log($"プレイヤーが{damage}ダメージ受けた");
        // 6番が無敵のレイヤー
        gameObject.layer = 6;
        //集中線パーティクルをPlay
        Camera.main.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        //画面を揺らす
        _source.GenerateImpulse();
        //プレイヤーを操作不能に
        _isMove = false;
        //プレイヤーを後ろに吹き飛ばす
        _rb.velocity = Vector3.zero;
        _rb.AddForce(new Vector3((!_playerFlip ? 1 : -1), 0.3f, 0) * _knockbackPower, ForceMode.Impulse);

        await UniTask.Delay(500);
        // 8番が通常時プレイヤーレイヤー
        gameObject.layer = 8;
        _isMove = true;
    }
}
