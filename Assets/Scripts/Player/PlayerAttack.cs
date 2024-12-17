using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;
using Cysharp.Threading.Tasks;

public class PlayerAttack : MonoBehaviour
{
    private readonly int Attack = Animator.StringToHash("Attack");
    private readonly int RangeAttack = Animator.StringToHash("RangeAttack");
    private Player _player;
    [Header("通常攻撃")]
    [SerializeField, InspectorVariantName("通常攻撃のゲームオブジェクト")] 
    private GameObject _attackCollider;
    [SerializeField, InspectorVariantName("上下攻撃の座標Y")]
    private float _attackPosY;
    [SerializeField, InspectorVariantName("クールタイム")]
    private float _attackCoolTime = 0.5f;
    private Vector3 _atkPos;
    private bool _canAttack = true;
    
    
    [Header("特殊攻撃")]
    [SerializeField, InspectorVariantName("ゲームオブジェクト")] 
    private GameObject _specialCollider;
    [SerializeField, InspectorVariantName("減る妖精ゲージ")]
    private float _spAttackDiminution = 150;

    public float SpDiminution => _spAttackDiminution;

    [Header("遠距離攻撃")]
    [SerializeField, InspectorVariantName("プレハブ")]
    private GameObject _rangeCollider;
    [SerializeField, InspectorVariantName("弾速")]
    private float _rangeAttackSpeed = 10;
    [SerializeField, InspectorVariantName("消えるまでの時間")]
    private float _lifeTime = 5;
    [SerializeField, InspectorVariantName("消費妖精ゲージ")]
    private float _rangeAttackDiminution = 100;
    [SerializeField, InspectorVariantName("クールタイム")]
    private float _rangeCoolTime = 1;
    private bool _canRangeAttack = true;
    
    
    private bool _attackAnimTrigger;
    private PlayerControls _controls;
    private bool _musicBoxPlaying;
    public bool MusicBoxPlayingSet {set => _musicBoxPlaying = value; }

    private void Awake()
    {
        _player = GetComponent<Player>();
        _controls = new PlayerControls();
        _controls.InGame.Attack.started += OnAttack;
        _controls.InGame.SpecialAttack.started += OnSpecialAttack;
        _controls.InGame.LongRangeAttack.canceled += OnLongRangeAttack;
        _player.AnimEvent.AnimEventDic.Add(PlayerAnimationEventController.animationType.AttackColliderEnable,AttackColliderSetActive);
        _player.AnimEvent.AnimEventDic.Add(PlayerAnimationEventController.animationType.AttackRangeEnable,RangeAttackInstantiate);
        _atkPos = _attackCollider.transform.localPosition;
    }
    
    private void OnDestroy()
    {
        _controls.Dispose();
        _controls.InGame.Attack.started -= OnAttack;
        _controls.InGame.SpecialAttack.started -= OnSpecialAttack;
        _controls.InGame.LongRangeAttack.canceled -= OnLongRangeAttack;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
    private async void OnAttack(InputAction.CallbackContext context)
    {
        if(!_canAttack) return;
        _canAttack = false;
        _player.Animator.SetTrigger(Attack);
        
        await UniTask.Delay(TimeSpan.FromSeconds(_attackCoolTime), cancellationToken: _player.CancellationToken);
        _canAttack = true;
    }
    
    private void AttackColliderSetActive()
    {
        // positionの設定
            _attackCollider.transform.localPosition =
                new Vector3(
                    _atkPos.x * (_player.PlayerMove.PlayerFlip ? 1 : -1), //左右の向き
                    Math.Sign(_player.PlayerMove.Dir.y) * _attackPosY + _atkPos.y, // 上下攻撃
                    _atkPos.z);
        _attackCollider.SetActive(true);
        PlayerEffectManager.Instance.PlayEffect(PlayEffectName.PlayerAttackEffect,
            Mathf.Approximately(gameObject.transform.GetChild(0).localEulerAngles.y, 180) ? 1 : 0);
        // 非アクティブは_attackCollider自身がする
    }

    private void OnSpecialAttack(InputAction.CallbackContext context)
    {
        if(!_player.PlayerStatus.CanUseFairyGauge(_spAttackDiminution))
            return; // 妖精ゲージが足りていなければ出せない
        if (!_player.PlayerMove.Dashing)
        {
            // 位置の固定
            _player.PlayerMove.IsFreeze = (true, true);
            // 特殊攻撃用当たり判定をアクティブにする
            _specialCollider.SetActive(true);
            // 非アクティブは_specialCollider自身がする
        }
    }

    public void SpecialCancel()
    {
        //攻撃食らい時などにキャンセルする
        _specialCollider.SetActive(false);
    }

    private async void OnLongRangeAttack(InputAction.CallbackContext context)
    {
        if (_musicBoxPlaying || // オルゴールの再生中でない
            !_player.PlayerStatus.IsLongRangeAttackRelease || // 遠距離攻撃が解放されていない
            !_player.PlayerStatus.CanUseFairyGauge(_rangeAttackDiminution) || // 必要な分ゲージがない
            !_canRangeAttack) //クールタイム中
            return;
        _canRangeAttack = false;
        //アニメーションの再生
        _player.Animator.SetTrigger(RangeAttack);
        RangeAttackInstantiate();
        _player.PlayerStatus.UseFairyGauge(_rangeAttackDiminution);
        
        _musicBoxPlaying = false;
        
        await UniTask.Delay(TimeSpan.FromSeconds(_rangeCoolTime), cancellationToken: _player.CancellationToken);
        _canRangeAttack = true;
    }

    private void RangeAttackInstantiate()
    {
        GameObject g = Instantiate(_rangeCollider);
        g.transform.up = new Vector3(_player.PlayerMove.PlayerFlip ? 1 : -1, 0, 0);
        g.transform.position = this.gameObject.transform.position;
        g.GetComponent<Rigidbody>().velocity = g.transform.up * _rangeAttackSpeed;
        Destroy(g, _lifeTime);
    }
}