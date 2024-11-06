using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    PlayerControls _controls;

    [SerializeField, InspectorVariantName("通常攻撃の攻撃判定")] private GameObject _attackCollider;
    [SerializeField, InspectorVariantName("特殊攻撃の攻撃判定")] private GameObject _spcialCollider;
    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.InGame.Attack.started += OnAttack;
        //_controls.InGame.SpecialAttack.started += OnSpecialAttack;
    }

    private void OnDestroy()
    {
        _controls.Dispose();
        _controls.InGame.Attack.started -= OnAttack;
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
        // 攻撃用当たり判定をアクティブにする
        _attackCollider.SetActive(true);
        // 非アクティブは_attackCollider自身がする
    }

    private void OnSpecialAttack(InputAction.CallbackContext context)
    {
        // 特殊攻撃用当たり判定をアクティブにする
        _spcialCollider.SetActive(true);
        // 非アクティブは_spcialCollider自身がする
    }
}
