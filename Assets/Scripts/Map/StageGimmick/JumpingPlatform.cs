using UnityEngine;

public class JumpingPlatform : MonoBehaviour
{
    //ジャンプ台
    [SerializeField, InspectorVariantName("ジャンプの強さ")]
    private float _jumpPower;

    void OnCollisionEnter(Collision other)
    {
        // 当たった相手のタグがPlayerだった場合
        if (other.gameObject.CompareTag("Player"))
        {
            // 当たった相手のRigidbodyコンポーネントを取得して、上向きの力を加える
            other.gameObject.GetComponent<Rigidbody>().AddForce(0, _jumpPower * 10, 0, ForceMode.Impulse);
        }
    }
}
