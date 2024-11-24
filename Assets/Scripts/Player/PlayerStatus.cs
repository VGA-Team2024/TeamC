using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerStatus : MonoBehaviour,IDamageable
{
    [SerializeField, InspectorVariantName("最大体力")]
    private int _maxHP = 5;
    [SerializeField, InspectorVariantName("現在体力")] 
    private int _currentHP;
    [SerializeField, InspectorVariantName("体力UIScript")]
    PlayerHealthUI healthUI;
    [SerializeField, InspectorVariantName("最大妖精ゲージ")] 
    private int _fairyGauge = 600;
    [SerializeField, InspectorVariantName("ダメージを受けた時に吹き飛ぶ力")] 
    private float _knockBackPower = 20;

    [SerializeField, InspectorVariantName("ダメージ後の無敵時間(秒)")] 
    private float _godTime = 0.5f;
    [SerializeField, InspectorVariantName("ダメージ後の移動不可時間(秒)")] 
    private float _NoMoveTime = 0.5f;
    [SerializeField, InspectorVariantName("無敵時間中のレイヤーの名前")]
    private string _godModeLayerName;

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

    public void TakeDamage(int damage)
    {
        Debug.Log($"プレイヤーが{damage}ダメージ受けた");
        // 無敵のレイヤーに変更
        gameObject.layer = LayerMask.NameToLayer(_godModeLayerName);
        //アニメーションの変更
        _player.Animator.SetTrigger("Damage");
        //集中線パーティクルをPlay
        Camera.main.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        //画面を揺らす
        _impulseSource.GenerateImpulse();
        //プレイヤーを操作不能に
        _player.PlayerMove.IsMove = false;
        //プレイヤーを後ろに吹き飛ばす
        _rb.velocity = Vector3.zero;
        _rb.AddForce(new Vector3((!_player.PlayerMove.PlayerFlip ? 1 : -1), 0.3f, 0) * _knockBackPower, ForceMode.Impulse);
        //体力を減らす
        _currentHP -= damage;
        //UIの更新
        if(healthUI)
            healthUI.PlayerHealthUpdate(_currentHP);
        //特殊攻撃を消す
        _player.PlayerAttack.SpecialCancel();
        
        //通常状態に復帰
        GodModeEnd();
        IsControl();
    }

    async void GodModeEnd()
    {
        await UniTask.Delay((int)(_godTime * 1000));
        // 通常レイヤーに戻す
        gameObject.layer = _normalLayer;
    }

    async void IsControl()
    {
        await UniTask.Delay((int)(_NoMoveTime*1000));
        _player.PlayerMove.IsMove = true;
    }
}
