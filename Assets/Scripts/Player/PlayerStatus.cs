using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamageable, IBlowable,ITechnicalable
{
    private readonly int Damage = Animator.StringToHash("Damage");

    [SerializeField, InspectorVariantName("最大体力")]
    private int _maxHP = 5;
    [SerializeField, InspectorVariantName("現在体力")] 
    private int _currentHP;
    [SerializeField, InspectorVariantName("最大妖精ゲージ")] 
    private int _maxFairyGauge = 600;
    [SerializeField, InspectorVariantName("現在妖精ゲージ")]
    private float _currentFairyGauge;
    [SerializeField, InspectorVariantName("一秒にゲージが回復する量")]
    private float _fairyGaugeParSecond = 20;
    [SerializeField, InspectorVariantName("吹き飛ぶ向き")]
    private Vector2 _knockBackDirection = new Vector2(1, 0.5f);
    [SerializeField, InspectorVariantName("ダメージを受けた時に吹き飛ぶ力")] 
    private float _knockBackPower = 20;

    [SerializeField, InspectorVariantName("ダメージ後の無敵時間(秒)")] 
    private float _godTime = 0.5f;
    [SerializeField, InspectorVariantName("ダメージ後の移動不可時間(秒)")] 
    private float _NoMoveTime = 0.5f;
    [SerializeField, InspectorVariantName("無敵時間中のレイヤーの名前")]
    private string _godModeLayerName;

    [SerializeField, InspectorVariantName("ダッシュの解放")]
    private bool _isDashRelease = true;
    public bool IsDashRelease => _isDashRelease;

    [SerializeField, InspectorVariantName("二段ジャンプの解放")]
    private bool _isSecondJumpRelease  = true;
    public bool IsSecondJumpRelease => _isSecondJumpRelease;

    [SerializeField, InspectorVariantName("遠距離攻撃の解放")]
    private bool _isLongRangeAttackRelease  = true;

    public bool IsLongRangeAttackRelease => _isLongRangeAttackRelease;

    private Cinemachine.CinemachineImpulseSource _impulseSource;
    private Player _player;
    int _normalLayer;
    private Rigidbody _rb;

    private void Start()
    {
        _currentHP = _maxHP; 
        _impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        _normalLayer = gameObject.layer;
        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _currentFairyGauge += Time.deltaTime * _fairyGaugeParSecond;
        if (_currentFairyGauge > _maxFairyGauge)
            _currentFairyGauge = _maxFairyGauge;
        if(_player.PlayerStatusUI)
            _player.PlayerStatusUI.FairyGaugeUpdate(_currentFairyGauge / _maxFairyGauge);
    }

    /// <summary>
    /// 妖精ゲージが足りているか判定する
    /// </summary>
    public bool CanUseFairyGauge(float value) => _currentFairyGauge >= value;

    /// <summary>
    /// 妖精ゲージを消費する関数
    /// </summary>
    /// <param name="diminution">消費量</param>
    public void UseFairyGauge(float diminution) => _currentFairyGauge -= diminution;
    
    public void TakeDamage(int damage)
    {
        // 無敵のレイヤーに変更
        gameObject.layer = LayerMask.NameToLayer(_godModeLayerName);
        //集中線パーティクルをPlay
        if(Camera.main.transform.GetChild(0).TryGetComponent<ParticleSystem>(out ParticleSystem line))
            line.Play();
        //画面を揺らす
        _impulseSource.GenerateImpulse();
        //プレイヤーを操作不能に
        _player.PlayerMove.IsMove = false;
        _rb.velocity = Vector3.zero;
        //体力を減らす
        _currentHP -= damage;
        //UIの更新
        if(_player.PlayerStatusUI)
            _player.PlayerStatusUI.PlayerHealthUpdate(_currentHP);
        //特殊攻撃を消す
        _player.PlayerAttack.SpecialCancel();
        
        //通常状態に復帰
        GodModeEnd();
        IsControl();
    }

    async void GodModeEnd()
    {
        await UniTask.Delay((int)(_godTime * 1000),cancellationToken: _player.CancellationToken);
        // 通常レイヤーに戻す
        gameObject.layer = _normalLayer;
    }

    async void IsControl()
    {
        await UniTask.Delay((int)(_NoMoveTime*1000),cancellationToken: _player.CancellationToken);
        _player.PlayerMove.IsMove = true;
    }

    public void BlownAway(Vector3 pos)
    {
        // アニメーションの変更
        _player.Animator.SetTrigger(Damage);
        // posのxの逆方向に飛ぶ
        Vector2 dir = transform.position.x - pos.x > 0 ?
            _knockBackDirection : 
            new Vector2(_knockBackDirection.x * -1, _knockBackDirection.y );
        _rb.AddForce(dir * _knockBackPower, ForceMode.Impulse);
    }

    public void setTechnical(Technical tech)
    {
        switch (tech)
        {
            case Technical.Dash:
                _isDashRelease = true;
                break;
            case Technical.SecondJump:
                _isSecondJumpRelease = true;
                break;
            case Technical.LongRangeAttack:
                _isLongRangeAttackRelease = true;
                break;
        }
    }
}
