using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField, Header("攻撃の持続時間")] private float _attackColliderTimer = 1f;
    // SetActiveがtrueになった0.2秒後にSetActiveをfalseに戻す
    async private void OnEnable()
    {
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
            damage.TakeDamage(1);
        }
    }
}
