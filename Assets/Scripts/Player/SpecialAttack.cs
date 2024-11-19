using UnityEngine;
using DG.Tweening;

public class SpecialAttack : MonoBehaviour
{
    private GameObject _player;
    private ITeleportable _playerTp;
    private PlayerMove _pm;
    private Vector3 _dir;
    [SerializeField,InspectorVariantName("行動終了までの時間")] 
    private float _timer = 1;
    private Vector3 _originLocalPos;
    
    private Tween _tw;
    private float _currentTimer;
    [SerializeField, InspectorVariantName("特殊攻撃の距離")] 
    private float _range = 8;

    private void Awake()
    {
        _pm = transform.parent.gameObject.GetComponent<PlayerMove>();
        _player = transform.parent.gameObject;
        _playerTp = _player.GetComponent<ITeleportable>();
        _originLocalPos = this.transform.localPosition;
    }

    private void OnEnable()
    {
        //位置の固定
        _pm.IsFreeze = true;
        // 針の向きの見た目の変更
        transform.localRotation = Quaternion.Euler(0, 0, (_pm.PlayerFlip ? 0 : 180));
        // 動く距離と向きの設定
        _dir = new Vector3((_pm.PlayerFlip ? 1 :-1), 0, 0) * _range;
        //DoTweenでの位置変更
        _tw = DOTween.To(() => 0f, x
            => _currentTimer = x,
            Mathf.PI, _timer
            ).OnUpdate(() => transform.transform.localPosition = _dir * Mathf.Sin(_currentTimer) + _originLocalPos)
            .OnComplete(() =>
            {
                this.gameObject.SetActive(false);
                _pm.IsFreeze = false;
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ITeleportable>(out ITeleportable tp))
        {
            Vector3 pos = other.transform.position;
            tp.Teleport(_player.transform.position);
            _playerTp.Teleport(pos);
            this.gameObject.SetActive(false);
            _tw.Complete();
        }
    }
}
