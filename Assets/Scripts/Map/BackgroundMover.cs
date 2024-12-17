using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 背景をスクロールする </summary>
public class BackgroundMover : MonoBehaviour
{
    private const float MaxLength = 1f;

    [SerializeField, Header("スクロール速度")] private Vector3 _offsetSpeed;
    
    private GameObject _followObj;
    private Material _material;
    private Vector3 _prevCameraPos; // 1フレーム前の位置

    private Action<Vector3> _moveAction; // 移動処理用のデリゲート

    private void Start()
    {
        _followObj = Camera.main.gameObject;
        SetUp();
        _prevCameraPos = _followObj.transform.position;
    }

    private void Update()
    {
        Vector3 currentPlayerPos = _followObj.transform.position;

        // 前回の位置と現在の位置の差分を計算
        Vector3 deltaPosition = currentPlayerPos - _prevCameraPos;

        // 移動処理
        _moveAction?.Invoke(deltaPosition);
        
        _prevCameraPos = currentPlayerPos;
    }

    // MaterialかTransformの移動処理を設定する
    private void SetUp()
    {
        // マテリアルがあれば取得(ImageかSpriteRenderer)
        if (TryGetComponent<Image>(out var image))
        {
            _material = image.material;
        }
        else if (TryGetComponent<SpriteRenderer>(out var sprite))
        {
            _material = sprite.material;
        }
        
        // 移動用メソッドを設定
        if (_material)
        {
            _moveAction = MoveMaterialOffset;
        }
        else
        {
            _moveAction = MoveTransform;
        }
    }

    // マテリアルのOffsetを変更する
    private void MoveMaterialOffset(Vector3 deltaPosition)
    {
        var x = Mathf.Repeat(_material.mainTextureOffset.x
                             + deltaPosition.x * _offsetSpeed.x, MaxLength);
        var y = Mathf.Repeat(_material.mainTextureOffset.y
                             + deltaPosition.y * _offsetSpeed.y, MaxLength);

        _material.mainTextureOffset = new Vector2(x, y);
    }
    
    // オブジェクトのTransformを変更する
    private void MoveTransform(Vector3 deltaPosition)
    {
        var x = deltaPosition.x *_offsetSpeed.x;
        var y = deltaPosition.y * _offsetSpeed.y;
            
        transform.position += new Vector3(-x, -y);
    }
    
    private void OnDestroy()
    {
        // ゲームをやめた後にマテリアルのOffsetを戻しておく
        if (_material)
        {
            _material.mainTextureOffset = Vector2.zero;
        }
    }
}
