using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
//落ちる床を制御するスクリプト
public class FallingFloor : MonoBehaviour
{
    [SerializeField,InspectorVariantName("落下スピード")] 
    private float _fallingSpeed;

    [SerializeField, InspectorVariantName("移動先のY座標")]
    private float _endPosition;

    [SerializeField, InspectorVariantName("プレイヤーが触れてから落下するまでの時間")]
    private float _delayTime;

    [SerializeField, InspectorVariantName("落下後の復活時間")]
    private float _spawnTime;

    private bool _isFalling = false;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = gameObject.transform.position;
    }

    //オブジェクトを動かすメソッド
    private void MoveFloor()
    {
        transform.position =
            Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, _endPosition, transform.position.z)
                , _fallingSpeed * Time.deltaTime);
        if (transform.position.y <= _endPosition)
        {
            ResetPos();
        }
    }

    private async void ResetPos()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_spawnTime));
        transform.position = _startPosition;
        _isFalling = false;
    }

    private void Update()
    {
        if (_isFalling)
        {
            MoveFloor();   
        }
    }

    private async void OnCollisionEnter(Collision other)
    {
        //触れたオブジェクトがPlayerタグ持っていたら少し時間をおいて_isFalling変数をtrueにする
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
            await UniTask.Delay(TimeSpan.FromSeconds(_delayTime));
            _isFalling = true;
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
