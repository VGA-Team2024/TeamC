using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour,IDamageable
{
    [SerializeField, InspectorVariantName("最大体力")] int _maxHP = 5;
    [SerializeField, InspectorVariantName("現在体力")] int _currentHP;
    [SerializeField, InspectorVariantName("最大妖精ゲージ")] int _fairyGauge = 600;
    [SerializeField, InspectorVariantName("ダメージを受けた時に吹き飛ぶ力")] private float _knockbackPower = 20;

    [SerializeField, InspectorVariantName("ダメージ後の無敵時間(秒)")] float _godTimer = 0.5f;
    [SerializeField, InspectorVariantName("ダメージ後の移動不可時間(秒)")] float _NoMoveTimer = 0.5f;

    Cinemachine.CinemachineImpulseSource _impulseSource;
    PlayerMove _pm;
    Rigidbody _rb;

    private void Start()
    {
        _impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        _pm = GetComponent<PlayerMove>();
        _rb = GetComponent<Rigidbody>();
    }

    public async void TakeDamage(int damage)
    {
        Debug.Log($"プレイヤーが{damage}ダメージ受けた");
        // 6番が無敵のレイヤー
        gameObject.layer = 6;
        //集中線パーティクルをPlay
        Camera.main.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        //画面を揺らす
        _impulseSource.GenerateImpulse();
        //プレイヤーを操作不能に
        _pm.IsMove = false;
        //プレイヤーを後ろに吹き飛ばす
        _rb.velocity = Vector3.zero;
        _rb.AddForce(new Vector3((!_pm.PlayerFlip ? 1 : -1), 0.3f, 0) * _knockbackPower, ForceMode.Impulse);

        GodModeEnd();
        IsControll();
    }

    async void GodModeEnd()
    {
        await UniTask.Delay(500);
        // 8番が通常時プレイヤーレイヤー
        gameObject.layer = 8;
    }

    async void IsControll()
    {
        await UniTask.Delay(500);
        _pm.IsMove = true;
    }
}
