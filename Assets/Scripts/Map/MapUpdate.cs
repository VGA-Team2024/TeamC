using System;
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
    private float _playerBoxSize_x;
    private float _playerBoxHalfSize_y;  // 全体のサイズを入力
    private float _portalBoxSize_x;
    private float _portalBoxHalfSize_y;
    
    public string StartMapName => _startMapName;
    
    private void Start()
    {
        Initialize();
        
        _triggerEvent.OnTriggerEnterAsObservable.Where(x => x.CompareTag("Portal"))
            .Subscribe(collider =>
        {
            HandleTriggerEnter(collider);
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
        var playerBox = _player.GetComponent<BoxCollider>();
        _playerBoxSize_x = playerBox.size.x;
        _playerBoxHalfSize_y = playerBox.size.y / 2;
        
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
        GetPairPortal(other, out var exit); // 入ったポータルから対のポータルを取得する
        
        if (!exit) return; // 対応したポータルがない場合は実行しない

        ExitPositionCalculator(exit);
        _playerMove.IsFreeze = (true, true); // プレイヤーの動きを制限
        
        // 画面フェードアウト(完全にフェードアウトしてからマップを切り替える)
        if (_fadeController) // Todo:フェードパネルがない状態でも挙動を確認できるようにするため。後で消す
        {
            await _fadeController.FadeOutAsync(_fadeDuration);
        }
        
        // ポータルに対応するマップの生成とカメラの設定
        foreach (var mapData in _mapManager.MapData)
        {
            if (mapData.EntranceColliders.Contains(other))
            {
                ChangeMapPrefab(mapData);
                SetCameraBoundingVolume(mapData);
                break;
            }
        }
        ToTeleportPlayer(exit.transform.position);
        
        // 画面フェードイン(カメラが完全に切り替わってから)
        if (_fadeController)  // Todo:フェードパネルがない状態でも挙動を確認できるようにするため。後で消す
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_bufferTIme), cancellationToken: destroyCancellationToken);
            _playerMove.IsFreeze = (false, false); // プレイヤーの移動制限を解除
            await _fadeController.FadeInAsync(_fadeDuration);
        }
    }

    // コライダーから出る位置を計算する
    private void ExitPositionCalculator(Collider exitCollider)
    {
        _portalBoxSize_x = exitCollider.transform.localScale.x;
        _portalBoxHalfSize_y = exitCollider.transform.localScale.y / 2;
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
        exitCollider = null; // 対応するポータル先がない場合
    }
    
    // プレイヤーをテレポートする
    private void ToTeleportPlayer(Vector3 position)
    {
        float diff = _playerBoxSize_x + _portalBoxSize_x;
        position.x += _playerMove.PlayerFlip ? diff : -diff;        // ポータルから確実に抜けるように位置をずらす
        position.y -= _portalBoxHalfSize_y - _playerBoxHalfSize_y;  // 移動した瞬間に浮かないようにする
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

    // カメラコライダーの更新
    private void SetCameraBoundingVolume(Map mapData)
    {
        _cameraSwitch.ChangeBoundingVolume(mapData.CameraCollider);
    }

    /// <summary> 指定の名前のマップをセットする(デバッグ用) </summary>
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
    
    /// <summary> 出口のポータルから現在のマップを設定する </summary>
    /// <param name="exitPortal"> 移動先のポータル </param>
    public void SetMapFromPortal(Collider exitPortal)
    {
        GetPairPortal(exitPortal, out var entrancePortal);
        foreach (var mapData in _mapManager.MapData)
        {
            if (mapData.EntranceColliders.Contains(entrancePortal))
            {
                ChangeMapPrefab(mapData);
                SetCameraBoundingVolume(mapData);
                ToTeleportPlayer(exitPortal.transform.position); // 設定した出口へ移動
            }
        }
    }
}