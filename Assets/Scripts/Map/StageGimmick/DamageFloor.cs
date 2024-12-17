using UnityEngine;

public class DamageFloor : MonoBehaviour
{
    [SerializeField,InspectorVariantName("ワープ先のオブジェクト")] GameObject _childObj;
    void OnTriggerEnter(Collider other)
    {
        //Playerタグを持つオブジェクトが触れたら子オブジェクトの位置に移動する
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = _childObj.transform.position;
        }

        if (other.TryGetComponent<IDamageable>(out IDamageable dmg))
        {
            dmg.TakeDamage(1);
        }

        CRIAudioManager.BGM.Play("SE_Gimmick","SE_Gimmick_PoisonSwanp01");
    }
}
