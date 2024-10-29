using UniRx;
using UnityEngine;

/// <summary> プレイヤーが接触したポータルを取得してマップを更新するクラス </summary>
public class MapUpdate : MonoBehaviour
{
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private OnTriggerEvent _triggerEvent;
    [SerializeField] private GameObject _player;            // 仮:プレイヤー
    [SerializeField] private GameObject _startMapPrefab;    // 仮：最初にいるマップ
    [SerializeField, Header("セットするマップの名前")] private string _startMapName;
    private GameObject _currentMapPrefab; // 現在いるマップデータ

    public string StartMapName => _startMapName;
    
    private void Start()
    {
        if (!_player.TryGetComponent<Rigidbody>(out _))
        {
            Debug.LogError("プレイヤーをにRigidbodyをアタッチしてください(プロトではコライダーのトリガーを使っているため)");
        }
        if (!_triggerEvent)
        {
            Debug.LogError("OnTriggerEventを持ったプレイヤーをアタッチしてください");
            return;
        }
        
        _triggerEvent.OnTriggerEnterAsObservable.Subscribe(collider =>
        {
            HandleTriggerEnter(collider);
        }).AddTo(this);

        _triggerEvent.OnTriggerExitAsObservable.Subscribe(collider =>
        {
            HandleTriggerExit(collider);
        }).AddTo(this);
        
        // マップセットしていない場合初期化
        if (!_currentMapPrefab)
        {
            _currentMapPrefab = _startMapPrefab; // 仮：最初にいるマップを設定
        }
    }

    // プレイヤーがポータルのトリガーに入ると呼ばれる(テレポート直後のトリガーも判定内)
    private void HandleTriggerEnter(Collider other)
    {
        // 入ったポータルから対のポータルを取得する
        GetPairPortal(other, out var exit);
        
        // 一度ポータルから出ていれば処理を行う
        if (_triggerEvent.LastPortalUsed != other)
        {
            // ポータルに対応するマップの生成
            foreach (var mapData in _mapManager.MapData)
            {
                if (mapData.EntranceColliders.Contains(other))
                {
                    ChangeMapPrefab(mapData);
                    break;
                }
            }
            ToTeleportPlayer(exit);
            _triggerEvent.LastPortalUsed = exit; // 最後に使用したポータルを記録
        }
    }

    // プレイヤーがポータルのトリガーから出ると呼ばれる
    private void HandleTriggerExit(Collider other)
    {
        // プレイヤーが最後に使ったポータルから離れたことを検知
        if (other == _triggerEvent.LastPortalUsed)
        {
            _triggerEvent.LastPortalUsed = null; // ポータルから離れたのでリセット
        }
    }
    
    //セットになっているポータル先を取得する
    private void GetPairPortal(Collider triggerCollider, out Collider exitCollider)
    {
        foreach (var portal in _mapManager.SpawnerPositionPair)
        {
            if (triggerCollider == portal.Portal_A)
            {
                exitCollider = portal.Portal_B;
                return;
            }

            if (triggerCollider == portal.Portal_B)
            {
                exitCollider = portal.Portal_A;
                return;
            }
        }
        
        // 対応するポータル先がない場合
        exitCollider = null;
    }
    
    // 仮：プレイヤーをテレポートする
    private void ToTeleportPlayer(Collider targetCollider)
    {
        _player.transform.position = targetCollider.transform.position;
    }

    private void ToTeleportPlayer(Vector3 position)
    {
        _player.transform.position = position;
    }

    // マップの更新
    private void ChangeMapPrefab(Map mapData)
    {
        if (_currentMapPrefab)
        {
            _currentMapPrefab.SetActive(false); // 前までいたマップを非アクティブ化
        }
        mapData.ExitMapPrefab.SetActive(true);  // ポータル先のマップをアクティブ化
        _currentMapPrefab = mapData.ExitMapPrefab; // 現在のマップを更新
    }

    /// <summary> 指定のマップをセットする </summary>
    /// <param name="mapName"> string マップのプレハブ名 </param>
    public void SetStartMap(string mapName)
    {
        foreach (var mapData in _mapManager.MapData)
        {
            if (mapData.ExitMapPrefab.name == mapName)
            {
                ChangeMapPrefab(mapData);
                ToTeleportPlayer(mapData.ExitMapPrefab.transform.position); // プレイヤーをマッププレハブのポジションへ移動
                return;
            }
            mapData.ExitMapPrefab.SetActive(false); // 関係ないマップを非アクティブにする
        }
        Debug.LogError("有効なマップ名を入力してください");
    }

    /// <summary> マップデータリストのインデックス0番にマップとプレイヤーの位置をセットする </summary>
    public void ResetStartMap()
    {
        var map = _mapManager.MapData[0];
        ChangeMapPrefab(map);
        ToTeleportPlayer(map.ExitMapPrefab.transform.position);
    }
}