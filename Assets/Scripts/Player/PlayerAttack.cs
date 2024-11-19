using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerAttack : MonoBehaviour
{
    private readonly int Attack = Animator.StringToHash("Attack");
    private PlayerControls _controls;
    private Player _player;
    [SerializeField, InspectorVariantName("通常攻撃の攻撃判定")] 
    private GameObject _attackCollider;
    [SerializeField, InspectorVariantName("特殊攻撃の攻撃判定")] 
    private GameObject _specialCollider;
    private bool _attackAnimTrigger;

    private void Awake()
    {
        _controls = new PlayerControls();
        _player = GetComponent<Player>();
        _controls.InGame.Attack.started += OnAttack;
        _controls.InGame.Attack.canceled += AttackCancel;
        _controls.InGame.SpecialAttack.started += OnSpecialAttack;
        _player.AnimationEvent.EventDictionary.Add("Attack" ,AttackColliderSetActive);
    }
    
    

    private void OnDestroy()
    {
        _controls.Dispose();
        _controls.InGame.Attack.started -= OnAttack;
        _controls.InGame.Attack.canceled -= AttackCancel;
        _controls.InGame.SpecialAttack.started -= OnSpecialAttack;
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
        // 非アクティブは_attackCollider自身がする
    }

    private void OnSpecialAttack(InputAction.CallbackContext context)
    {
        // 特殊攻撃用当たり判定をアクティブにする
        _specialCollider.SetActive(true);
        // 非アクティブは_specialCollider自身がする
    }
}
