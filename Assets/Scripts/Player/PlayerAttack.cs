using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private readonly int Attack = Animator.StringToHash("Attack");
    private PlayerControls _controls;
    private Player _player;
    [SerializeField, InspectorVariantName("通常攻撃のゲームオブジェクト")] 
    private GameObject _attackCollider;
    [SerializeField, InspectorVariantName("特殊攻撃のゲームオブジェクト")] 
    private GameObject _specialCollider;
    [SerializeField, InspectorVariantName("遠距離攻撃のプレハブ")]
    private GameObject _rangeCollider;

    [SerializeField, InspectorVariantName("遠距離攻撃速度")]
    private float _rangeAttackSpeed = 10;

    [SerializeField, InspectorVariantName("遠距離攻撃が消えるまでの時間")]
    private float _lifeTime = 5;
    private bool _attackAnimTrigger;

    private bool _musicBoxPlaying;

    private void Awake()
    {
        _controls = new PlayerControls();
        _player = GetComponent<Player>();
        _controls.InGame.Attack.started += OnAttack;
        _controls.InGame.Attack.canceled += AttackCancel;
        _controls.InGame.SpecialAttack.started += OnSpecialAttack;
        _controls.InGame.LongRangeAttack.canceled += OnLongRangeAttack;
        _controls.InGame.MusicBox.performed += ((c) => _musicBoxPlaying = true);
        _player.AnimationEvent.EventDictionary.Add("Attack" ,AttackColliderSetActive);
    }
    
    private void OnDestroy()
    {
        _controls.Dispose();
        _controls.InGame.Attack.started -= OnAttack;
        _controls.InGame.Attack.canceled -= AttackCancel;
        _controls.InGame.SpecialAttack.started -= OnSpecialAttack;
        _controls.InGame.LongRangeAttack.canceled -= OnLongRangeAttack;
        _controls.InGame.MusicBox.performed -= ((c) => _musicBoxPlaying = true);
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Dispose();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        _attackAnimTrigger = true;
        _player.Animator.SetBool(Attack,_attackAnimTrigger);
        _attackAnimTrigger = false;
    }

    private void AttackCancel(InputAction.CallbackContext context)
    {
        _attackAnimTrigger = false;
        _player.Animator.SetBool(Attack,_attackAnimTrigger);
    }

    private void AttackColliderSetActive()
    {
        // 攻撃用当たり判定をアクティブにする
        Vector3 atkPos = _attackCollider.transform.localPosition;
        _attackCollider.transform.localPosition = new Vector3(Mathf.Abs(atkPos.x) * (_player.PlayerMove.PlayerFlip ? 1 : -1),atkPos.y, atkPos.z);
        _attackCollider.SetActive(true);
        ParticleSystem p = _attackCollider.GetComponent<ParticleSystem>();
        var mainModule = p.main;
        mainModule.startRotationYMultiplier = _player.PlayerMove.PlayerFlip ? 1 : -1;
        p.Play();
        EffectManager.Instance.PlayEffect( (int)PlayEffectName.PlayerAttackEffect,
            Mathf.Approximately(gameObject.transform.GetChild(0).localEulerAngles.y, 180) ? 1 : 0);
        // 非アクティブは_attackCollider自身がする
    }
    

    private void OnSpecialAttack(InputAction.CallbackContext context)
    {
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

    private void OnLongRangeAttack(InputAction.CallbackContext context)
    {
        if (!_musicBoxPlaying)
        {
            // ToDo: AnimationEventで呼ぶようにする
            RangeAttackInstantiate();
        }
        _musicBoxPlaying = false;
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