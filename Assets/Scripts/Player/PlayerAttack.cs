using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class PlayerAttack : MonoBehaviour
{
    private readonly int CanAttackAnim = Animator.StringToHash("CanAttack");
    private readonly int Attack = Animator.StringToHash("Attack");
    private readonly int Throw = Animator.StringToHash("Throw");
    private readonly int Vertical = Animator.StringToHash("Vertical");
    private readonly int RangeAttack = Animator.StringToHash("RangeAttack");
    private Player _player;
    [Header("通常攻撃")]
    [SerializeField, InspectorVariantName("通常攻撃のゲームオブジェクト")] 
    private GameObject _attackCollider;
    [SerializeField, InspectorVariantName("自身の吹き飛び")]
    private Vector2 _hitKnockBack;
    [SerializeField, InspectorVariantName("吹き飛び時間")]
    private float _knockBackTimer = 0.1f;
    [SerializeField, InspectorVariantName("上下攻撃の座標Y")]
    private float _attackPosY;
    [SerializeField, InspectorVariantName("クールタイム")]
    private float _attackCoolTime = 0.5f;
    private Vector3 _atkPos;
    private bool _canAttack = true;
    public bool CanAttack => _canAttack;
    
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

    private float _axisY;
    private bool _attackAnimTrigger;
    private PlayerControls _controls;


    private void Awake()
    {
        _player = GetComponent<Player>();
        _controls = new PlayerControls();
        _controls.InGame.Attack.started += OnAttack;
        _controls.InGame.SpecialAttack.started += OnSpecialAttack;
        _controls.InGame.LongRangeAttack.canceled += OnLongRangeAttack;
        _controls.InGame.Vertical.started += OnVertical;
        _controls.InGame.Vertical.performed += OnVertical;
        _controls.InGame.Vertical.canceled += OnVertical;
        _player.AnimEvent.AnimEventDic.Add(PlayerAnimationEventController.animationType.AttackColliderEnable,AttackColliderSetActive);
        _player.AnimEvent.AnimEventDic.Add(PlayerAnimationEventController.animationType.AttackRangeEnable,RangeAttackInstantiate);
        _player.AnimEvent.AnimEventDic.Add(PlayerAnimationEventController.animationType.AttackSpThrow,() => _specialCollider.SetActive(true));
        _atkPos = _attackCollider.transform.localPosition;
    }
    
    private void OnDestroy()
    {
        _controls.Dispose();
        _controls.InGame.Attack.started -= OnAttack;
        _controls.InGame.SpecialAttack.started -= OnSpecialAttack;
        _controls.InGame.LongRangeAttack.canceled -= OnLongRangeAttack;
        _controls.InGame.Vertical.started -= OnVertical;
        _controls.InGame.Vertical.performed -= OnVertical;
        _controls.InGame.Vertical.canceled -= OnVertical;
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
        _player.Animator.SetBool(CanAttackAnim,true);
    }

    private async void OnAttack(InputAction.CallbackContext context)
    {
        if(!_canAttack) return;
        _canAttack = false;
        _player.Animator.SetBool(CanAttackAnim,false);
        _player.Animator.SetTrigger(Attack);
        
        await UniTask.Delay(TimeSpan.FromSeconds(_attackCoolTime), cancellationToken: _player.CancellationToken);
        _player.Animator.SetBool(CanAttackAnim,true);
        _canAttack = true;
    }

    private void OnVertical(InputAction.CallbackContext context)
    {
        _axisY = context.ReadValue<float>();
        _player.Animator.SetFloat(Vertical,_axisY);
    }
    
    private void AttackColliderSetActive()
    {
        // positionの設定
        _attackCollider.transform.localPosition =
            new Vector3(
                _atkPos.x * (_player.PlayerMove.PlayerFlip ? 1 : -1), //左右の向き
                _atkPos.y, // 上下攻撃
                _atkPos.z);

        if (_axisY != 0)
        {
            if (_axisY > 0)
            {// 上入力
                _attackCollider.transform.localPosition = new Vector2(0, _attackPosY);
                PlayerEffectManager.Instance.PlayEffect(PlayEffectName.PlayerAttackEffectUp,0);
            }
            else if (!_player.PlayerMove.IsGround)
            {// 下入力かつ空中
                _attackCollider.transform.localPosition = new Vector2(0, _attackPosY * -1);
                PlayerEffectManager.Instance.PlayEffect(PlayEffectName.PlayerAttackEffectDown, 0);
            }
        }
        else
        {
            //横入力
            PlayerEffectManager.Instance.PlayEffect(PlayEffectName.PlayerAttackEffect,
                Mathf.Approximately(gameObject.transform.GetChild(1).localEulerAngles.y, 180) ? 1 : 0);
        }
        _attackCollider.SetActive(true);
        // 非アクティブは_attackCollider自身がする
    }

    public async void HitKnockBack()
    {
        _player.PlayerMove.IsMove = false;
        _player.Rigidbody.velocity = Vector3.zero;
        _player.Rigidbody.AddForce(_player.PlayerMove.PlayerFlip ? 
            _hitKnockBack : 
            new Vector2( _hitKnockBack.x *-1, _hitKnockBack.y)
            , ForceMode.Impulse);
        await UniTask.Delay((TimeSpan.FromSeconds(_knockBackTimer)),cancellationToken: _player.CancellationToken);
        _player.PlayerMove.IsMove = true;
    }

    private void OnSpecialAttack(InputAction.CallbackContext context)
    {
        if(!_player.PlayerStatus.CanUseFairyGauge(_spAttackDiminution) || _specialCollider.activeSelf)
            return; // 妖精ゲージが足りていなければ出せない
        if (!_player.PlayerMove.Dashing)
        {
            // 位置の固定
            _player.PlayerMove.IsFreeze = (true, true);
            // アニメーションの再生　　　　　_specialColliderのActiveはAnimationEventで行う
            _player.Animator.SetTrigger(Throw);
            // ジャンプのキャンセル
            _player.PlayerMove.JumpTokenCancel();
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
        if (!_player.PlayerMusicBox.MusicBoxPlaying && // オルゴールが再生中でない
            _player.PlayerStatus.IsLongRangeAttackRelease && // 遠距離攻撃が解放されている
            _player.PlayerStatus.CanUseFairyGauge(_rangeAttackDiminution) && // ゲージが十分
            _canRangeAttack) //クールタイム中でない
        {
            _canRangeAttack = false;
            //アニメーションの再生
            _player.Animator.SetTrigger(RangeAttack);
            RangeAttackInstantiate();
            _player.PlayerStatus.UseFairyGauge(_rangeAttackDiminution);
            _player.PlayerMusicBox.MusicBoxPlaying = false;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_rangeCoolTime), cancellationToken: _player.CancellationToken);
            _canRangeAttack = true;
        }
        else
        {
            _player.PlayerMusicBox.MusicBoxPlaying = false;
        }
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