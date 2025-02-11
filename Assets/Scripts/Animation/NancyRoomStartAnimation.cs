using Cysharp.Threading.Tasks;
using UnityEngine;

// ナンシーの部屋に入った時に一度だけ再生されるAnimation
public class NancyRoomStartAnimation : MonoBehaviour
{
   [SerializeField] private MapUpdate _mapUpdate;
   [SerializeField] private SearchArea _searchArea;
   [SerializeField] private string _cueName;
    
    private PlayerMove _playerMove;
    private Collider _collider;
    public void OnDisable()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        _playerMove = _mapUpdate.PlayerMove;
        _collider = _searchArea.GetComponent<Collider>();
    }

    // ToDo : 非同期対応　Fade対応　ライトの対応　エフェクトがほしい
    public async void PlayAnimation()
    {
        // Playerの入力を無効化する
        _playerMove.IsFreeze = (true, true);
        // nancyの動きを止めて特定のアニメーションを行う
        _collider.gameObject.SetActive(false);
        // Playerを動かす
        await PlayerAnimation();
        // ボイスを再生する
        CRIAudioManager.VOICE.Play("Voice", _cueName);
        // コライダーをアクティブにする
        _collider.gameObject.SetActive(true);
    }

    // メアリーのアニメーションを設定
    private async UniTask PlayerAnimation()
    {
        
    }
}
