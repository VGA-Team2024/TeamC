using UnityEngine;
public class MoveFloor : MonoBehaviour
{
    [SerializeField, InspectorVariantName("スタート地点")] private Vector3 _startPosition;     //スタート地点
    [SerializeField, InspectorVariantName("折り返し地点")] private Vector3 _returnPosition;    //折り返し地点
    [SerializeField,InspectorVariantName("移動スピード")] private float _moveSpeed;            //床の移動スピード
    private bool _isReturn;                                                                  //移動先の切り替えフラグ

    void OnValidate()
    {
        transform.position = _startPosition;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //_returnPositionと自身の距離が0.1未満なら_isReturnをtrue
        if (Vector3.Distance(_returnPosition, transform.position) < 0.1f)
        {
            _isReturn = true;
        }
        //_returnPositionと自身の距離が0.1未満なら_isReturnをfalse
        else if (Vector3.Distance(_startPosition, transform.position) < 0.1f)
        {
            _isReturn = false;
        }
        
        //_isReturnがfalseの時は_returnPositionの方向に移動する
        if (!_isReturn)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _returnPosition, _moveSpeed * Time.deltaTime);
        }
        //_isReturnがtrueの時は_startPositionの方向に移動する
        else
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _startPosition, _moveSpeed * Time.deltaTime);
        }
    }
    

    private void OnCollisionEnter(Collision other)
    {
        //プレイヤータグのついたオブジェクトを子オブジェクトにする
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        //子オブジェクトから外す
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
