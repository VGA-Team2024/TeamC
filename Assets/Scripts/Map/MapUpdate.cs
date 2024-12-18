using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

/// <summary> プレイヤーが接触したポータルを取得してマップを更新するクラス </summary>
public class MapUpdate : MonoBehaviour
{
    [SerializeField] private MapManager _mapManager;
    [SerializeField, Header("プレイヤー")] private GameObject _player;            // 仮:プレイヤー
    [SerializeField, Header("最初のステージプレハブ")] private GameObject _startMapPrefab;    // 仮：最初にいるマップ
    [SerializeField, Header("セットするマップの名前")] private string _startMapName;
    
    [Header("マップ移動時のフェード処理")]
    [SerializeField, InspectorVariantName("フェードにかける時間")]　private float _fadeDuration = 1f;
    [SerializeField, InspectorVariantName("インとアウトの間の時間")]　private float _bufferTIme = 0.5f;
    
    private GameObject _currentMapPrefab; // 現在いるマップデータ
    private CameraSwitch _cameraSwitch;
    private FadeController _fadeController;
    private OnTriggerEvent _triggerEvent;
    private PlayerMove _playerMove;
    private Collider _entrance;

    public string StartMapName => _startMapName;
    
    private void Start()
    {
        Initialize();
        
        _triggerEvent.OnTriggerEnterAsObservable
            .ThrottleFirst(TimeSpan.FromSeconds(_fadeDuration + _bufferTIme))
            .Subscribe(collider =>
        {
            HandleTriggerEnter(collider);
        }).AddTo(this);

        _triggerEvent.OnTriggerExitAsObservable
            .Buffer(2, 1)
            .Select(portals => portals.Last())
            .Subscribe(collider =>
        {
            HandleTriggerExit(collider);
        }).AddTo(this);
        
        // マップセットしていない場合初期化
        if (!_currentMapPrefab)
        {
            _currentMapPrefab = _startMapPrefab; // 仮：最初にいるマップを設定
        }
    }

    private void Initialize()
    {
        _mapManager = FindObjectOfType<MapManager>();
        _cameraSwitch = FindObjectOfType<CameraSwitch>();
        _fadeController = FindObjectOfType<FadeController>();
        _triggerEvent = _player.GetComponent<OnTriggerEvent>();
        _playerMove = _player.GetComponent<PlayerMove>();
        
        if (!_fadeController)
        {
            Debug.LogError("SystemCanvasプレハブをシーン上に配置してください");
        }
        if (!_cameraSwitch)
        {
            Debug.LogError("CameraSwitchコンポーネントが付いたオブジェクト一つをシーン上に配置してください");
        }
        if (!_player.TryGetComponent<Rigidbody>(out _))
        {
            Debug.LogError("プレイヤーをにRigidbodyをアタッチしてください(プロトではコライダーのトリガーを使っているため)");
        }
        if (!_triggerEvent)
        {
            Debug.LogError("OnTriggerEventを持ったプレイヤーをアタッチしてください");
        }
    }

    // プレイヤーがポータルのトリガーに入ると呼ばれる(テレポート直後のトリガーも判定内)
    private async UniTask HandleTriggerEnter(Collider other)
    {
        // 入ったポータルから対のポータルを取得する
        GetPairPortal(other, out var exit);
        
        if (!exit) return; // 対応したポータルがない場合は実行しない

        _playerMove.IsFreeze = (true, true); // プレイヤーの動きを制限
        
        // 画面フェードアウト(完全にフェードアウトしてからマップを切り替える)
        if (_fadeController) // Todo:フェードパネルがない状態でも挙動を確認できるようにするため。後で消す
        {
            _fadeController.FadeOut(_fadeDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration), cancellationToken: destroyCancellationToken);        
        }
        
        // 一度ポータルから出ていれば処理を行う
        if (_triggerEvent.LastPortalUsed != other)
        {
            // ポータルに対応するマップの生成
            foreach (var mapData in _mapManager.MapData)
            {
                if (mapData.EntranceColliders.Contains(other))
                {
                    ChangeMapPrefab(mapData);
                    // ToDo:カメラのコライダーを変更する
                    _cameraSwitch.ChangeBoundingVolume(mapData.CameraCollider);
                    break;
                }
            }
            ToTeleportPlayer(exit.transform.position);
            _triggerEvent.LastPortalUsed = exit; // 最後に使用したポータルを記録
            
            // 画面フェードイン(カメラが完全に切り替わってから)
            if (_fadeController)  // Todo:フェードパネルがない状態でも挙動を確認できるようにするため。後で消す
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_bufferTIme), cancellationToken: destroyCancellationToken);
                _fadeController.FadeIn(_fadeDuration);
            }
            _playerMove.IsFreeze = (false, false); // プレイヤーの移動制限を解除
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