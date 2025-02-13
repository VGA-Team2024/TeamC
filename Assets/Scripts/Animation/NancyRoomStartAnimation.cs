using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

// ナンシーの部屋に入った時に一度だけ再生されるAnimation
public class NancyRoomStartAnimation : MonoBehaviour
{
   [SerializeField] private MapUpdate _mapUpdate;
   [SerializeField] private SearchArea _searchArea;
   [SerializeField] private string _cueName;
   [SerializeField] private Vector3 _playerMovePos;
   [SerializeField] private float _playerMoveAnimTime;

    private PlayerMove _playerMove;
    private Collider _collider;
    bool _animPlaying = false;
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
        _animPlaying = true;
        // Playerの入力を無効化する
        _playerMove.IsMove = false;
        // nancyの動きを止めて特定のアニメーションを行う
        _collider.gameObject.SetActive(false);
        // Playerを動かす
        await PlayerAnimation();
        Debug.Log("MoveComplete");
        // ボイスを再生する
        CRIAudioManager.VOICE.Play("Voice", _cueName);
        // コライダーをアクティブにする
        _collider.gameObject.SetActive(true);
        // Playerの入力を有効にする
        _playerMove.IsMove = true;
    }

    // メアリーのアニメーションを設定
    private async UniTask PlayerAnimation()
    {
        Rigidbody rb = _playerMove.GetComponent<Rigidbody>();
        // rigidbodyのDOMoveはisKinematicをtrueにしないとvelocityが動かない?
        rb.isKinematic = true;
        await rb.DOMove(_playerMovePos,_playerMoveAnimTime,false).SetEase(Ease.Linear);
        rb.isKinematic = false;
    }

    //適当に作ったやつ 着地判定がないと変な動きする
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _playerMove.IsGround && _animPlaying == false)
        {
            PlayAnimation();
        }
    }
}
