using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    // SetActiveがtrueになった0.2秒後にSetActiveをfalseに戻す
    async private void OnEnable()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        this.gameObject.SetActive(false);
    }

    // 敵に当たった初めのフレームで敵のTakeDamageを呼ぶ
    private void OnTriggerEnter(Collider other)
    {
        // if(当たったコライダーが敵の物なら～)

        // 当たったコライダーのゲームオブジェクトにIDamageableがついているなら
        if(other.TryGetComponent<IDamageable>(out IDamageable damage))
        {
            damage.TakeDamage(1);
        }
    }
}
