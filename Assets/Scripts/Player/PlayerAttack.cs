using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    PlayerControls _controls;
    PlayerMove _pm;
    [SerializeField, InspectorVariantName("通常攻撃の攻撃判定")] 
    private GameObject _attackCollider;
    [SerializeField, InspectorVariantName("特殊攻撃の攻撃判定")] 
    private GameObject _spcialCollider;
    private void Awake()
    {

        _controls = new PlayerControls();
        _pm = GetComponent<PlayerMove>();
        _controls.InGame.Attack.started += OnAttack;
        _controls.InGame.SpecialAttack.started += OnSpecialAttack;
    }

    private void OnDestroy()
    {
        _controls.Dispose();
        _controls.InGame.Attack.started -= OnAttack;
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
        _pm.Animator.SetTrigger("Attack");
        //AttackColliderSetActive();
    }

    public void AttackColliderSetActive()
    {
        // 攻撃用当たり判定をアクティブにする
        Vector3 atkPos = _attackCollider.transform.localPosition;
        _attackCollider.transform.localPosition = new Vector3(Mathf.Abs(atkPos.x) * (_pm.PlayerFlip ? 1 : -1),atkPos.y, atkPos.z);
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
