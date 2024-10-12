using UnityEngine;
using UnityEngine.UI;

public class BackgroudMover : MonoBehaviour
{
    private const float MaxLength = 1f;
    private const string PropName = "_MainTex";

    [SerializeField, Header("スクロール速度")] private Vector3 _offsetSpeed;
    [SerializeField] private GameObject _player;    // 何かしらの方法でプレイヤーを取得する
    
    private Material _material;
    private Vector3 _prevPlayerPos; // 1フレーム前の位置
    
    private void Start()
    {
        // イメージコンポーネントからマテリアルを取得
        if (GetComponent<Image>() is { } i)   // is Image i // not null
        {
            _material = i.material;
        }
        
        // プレイヤーの初期位置を保存
        _prevPlayerPos = _player.transform.position;
    }

    private void Update()
    {
        if (_material)
        {
            // 現在のプレイヤーの位置を取得
            Vector3 currentPlayerPos = _player.transform.position;

            // 前回の位置と現在の位置の差分を計算
            Vector3 deltaPosition = currentPlayerPos - _prevPlayerPos;

            // 差分に基づいてテクスチャのオフセットを計算
            var x = Mathf.Repeat(_material.GetTextureOffset(PropName).x
                                 + deltaPosition.x * _offsetSpeed.x, MaxLength);
            var y = Mathf.Repeat(_material.GetTextureOffset(PropName).y
                                 + deltaPosition.y * _offsetSpeed.y, MaxLength);

            // テクスチャのオフセットを更新
            _material.SetTextureOffset(PropName, new Vector2(x, y));

            // 現在の位置を前回の位置として保存
            _prevPlayerPos = currentPlayerPos;
        }
        
    }
    
    private void OnDestroy()
    {
        // ゲームをやめた後にマテリアルのOffsetを戻しておく
        if (_material)
        {
            _material.SetTextureOffset(PropName, Vector2.zero);
        }
    }
}

