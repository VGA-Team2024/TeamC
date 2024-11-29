using DG.Tweening;
using UnityEngine;
//落ちる床を制御するスクリプト
public class FallingFloor : MonoBehaviour
{
    [SerializeField,InspectorVariantName("落下スピード")] 
    private float _fallingSpeed;

    [SerializeField, InspectorVariantName("移動先のY座標")]
    private float _endPosition;

    [SerializeField, InspectorVariantName("プレイヤーが触れてから落下するまでの時間")]
    private float _delayTime;
    
    //オブジェクトを動かすメソッド
    void MoveFloor()
    {
        transform.DOLocalMoveY(_endPosition, _fallingSpeed).OnComplete(() => {Destroy(gameObject);}).SetDelay(_delayTime);
    }

    void OnCollisionEnter(Collision other)
    {
        //触れたオブジェクトがPlayerタグ持っていたら少し時間をおいて_isFalling変数をtrueにする
        if (other.gameObject.CompareTag("Player"))
        {
            MoveFloor();
            other.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision other)
    {
        //子オブジェクトから外す
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
