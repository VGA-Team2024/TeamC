using UnityEngine;
using UnityEngine.UI;

/// <summary> インゲームのチャプター機能用のボタンにアタッチする </summary>
public class MapSetter : MonoBehaviour
{
    [SerializeField, InspectorVariantName("移動先のポータル")] private Collider _portal;
    [SerializeField, InspectorVariantName("ポータルの右から出る")] private bool _dirRight;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private MapUpdate _mapUpdate;
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private Button _button;

    private void Awake()
    {
        _mapManager = FindObjectOfType<MapManager>();
        _mapUpdate = FindObjectOfType<MapUpdate>();
        _playerMove = FindObjectOfType<PlayerMove>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SkipMap);
        nullCheck();
    }

    // MapManagerからMap移動の機能を呼び出す
    public void SkipMap()
    {
        _playerMove.PlayerFlip = _dirRight; // プレイヤーの向きを変える
        _mapUpdate.SetMapFromPortal(_portal);
        _mapManager.CanvasActivateFlag = false; // UIを消す
        _playerMove.IsFreeze = (false, false); // プレイヤーの移動制限を解除(制限している場合)
    }

    private void nullCheck()
    {
        if (_mapManager == null)
        {
            Debug.LogWarning($"{_mapManager}が取得できませんでした({gameObject.name})");
        }
        
        if (_mapUpdate == null)
        {
            Debug.LogWarning($"{_mapUpdate}が取得できませんでした({gameObject.name})");
        }
        
        if (_playerMove == null)
        {
            Debug.LogWarning($"{_playerMove}が取得できませんでした({gameObject.name})");
        }
    }
}
