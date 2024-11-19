﻿using Cysharp.Threading.Tasks;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField, Header("攻撃の持続時間")] private float _attackColliderTimer = 1f;
    // SetActiveがtrueになった_attackColliderTimer秒後にSetActiveをfalseに戻す
    private Player _player;
    private Action _hitSoundPlay;

    async private void OnEnable()
    {
        if(!_player) _player = transform.parent.GetComponent<Player>();
        _player.PlayerSounds.PlayerSEPlay(PlayerSoundEnum.Attack);
        await UniTask.Delay(TimeSpan.FromSeconds(_attackColliderTimer));
        this.gameObject.SetActive(false);
    }

    // 敵に当たった初めのフレームで敵のTakeDamageを呼ぶ
    private void OnTriggerEnter(Collider other)
    {
        // 同じタグ同士ならダメージを与えないようreturnする
        if(gameObject.CompareTag(other.gameObject.tag))
        {
            return;
        }

        // 当たったコライダーのゲームオブジェクトにIDamageableがついているなら
        if(other.TryGetComponent<IDamageable>(out IDamageable damage))
        {
            _player.PlayerSounds.PlayerSEPlay(PlayerSoundEnum.AttackHit);
            damage.TakeDamage(1);
        }
    }
}
