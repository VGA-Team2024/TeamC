using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
public class PlayerMove : MonoBehaviour ,IDamageable
{
    Rigidbody _rb;
    PlayerControls _controls;
    Vector2 _dir; //ActionMapのMoveの値を保存するVector2
    [SerializeField, Header("プレイヤーの移動速度")] float _moveSpeed = 2;
    [SerializeField, Header("プレイヤーのジャンプ力")] float _jumpPower = 5;
    [SerializeField, Header("攻撃時に出すゲームオブジェクト")] GameObject _attackCollider;
    [SerializeField, Header("着地判定用Rayの長さ")] float _rayLength = 0.55f;

    Cinemachine.CinemachineImpulseSource source;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        source = GetComponent<Cinemachine.CinemachineImpulseSource>();

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


    void OnJump(InputAction.CallbackContext context)
    {
        // 地面についているかの判定
        // タグの判定などをしていないので敵の上に居るときでもジャンプが可能になっている
        if (Physics.Raycast(this.transform.position, Vector3.down, _rayLength))
        {
            _rb.AddForce(Vector2.up * _jumpPower, ForceMode.Impulse);
        }
    }


    void OnAttack(InputAction.CallbackContext context)
    {
        // 攻撃用当たり判定をアクティブにする
        _attackCollider.SetActive(true);
        // 非アクティブは_attackCollider自身がする
    }

    // ActionMapのMove
    void OnMove(InputAction.CallbackContext context)
    {
        _dir = context.ReadValue<Vector2>();
        //今向いている方向と逆に向いたときに攻撃判定用ゲームオブジェクトの位置を変える
        if((_dir.x > 0 && _attackCollider.transform.localPosition.x < 0)
            ||(_dir.x < 0 && _attackCollider.transform.localPosition.x > 0))
        {
            Vector3 acp = _attackCollider.transform.localPosition;
            _attackCollider.transform.localPosition = new Vector3(acp.x * - 1, acp.y, acp.z);
        }
    }

    private void FixedUpdate()
    {
        //ジャンプ判定用Rayの表示
        Debug.DrawRay(this.transform.position, Vector3.down * _rayLength,Color.black);
        
        //左右移動
        _rb.velocity = new Vector3(_dir.x * _moveSpeed,_rb.velocity.y ,0);
    }

    // 攻撃されたときに呼ばれる
    public async void TakeDamage(int damage)
    {
        Debug.Log($"プレイヤーが{damage}ダメージ受けた");
        // 6番が無敵のレイヤー
        this.gameObject.layer = 6;
        Camera.main.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        source.GenerateImpulse();

        await UniTask.Delay(500);
        // 8番が通常時プレイヤーレイヤー
        this.gameObject.layer = 8;
    }


}
