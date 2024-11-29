using UnityEngine;
//落ちる床を制御するスクリプト
public class FallingFloor : MonoBehaviour
{
    [SerializeField,InspectorVariantName("落下スピード")] 
    float _fallingSpeed;
    
    bool _isFalling;
    
    void Awake()
    {
        _isFalling = false;
    }

    void Update()
    {
        GroundCheck();
        if (_isFalling)
        {
            MoveFloor();
        }
    }
    
    //地面に触れたかを確認するメソッド
    void GroundCheck()
    {
        //Rayが地面に触れたら自身をデストロイする
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit floorRay, 2))
        {
            if (!floorRay.collider.gameObject.CompareTag("Player"))
            {
                transform.SetParent(null);
                // Destroy(gameObject);
            }
        }
    }
    
    //オブジェクトを動かすメソッド
    void MoveFloor()
    {
        transform.position =
            Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,-1000,transform.position.z), _fallingSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        //触れたオブジェクトがPlayerタグ持っていたら少し時間をおいて_isFalling変数をtrueにする
        if (other.gameObject.CompareTag("Player"))
        {
            _isFalling = true;
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
