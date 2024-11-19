using UnityEngine;
using DG.Tweening;

public class SpecialAttack : MonoBehaviour
{
    private Player _player;
    private ITeleportable _parentTp;
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
        _parentTp = transform.parent.gameObject.GetComponent<ITeleportable>();
        _originLocalPos = this.transform.localPosition;
        _player = transform.parent.gameObject.GetComponent<Player>();
    }

    private void OnEnable()
    {
        //位置の固定
        _player.PlayerMove.IsFreeze = true;
        //音の再生
        _player.PlayerSounds.PlayerSEPlay(PlayerSoundEnum.ThrowNeedle);
        // 針の向きの見た目の変更
        transform.localRotation = Quaternion.Euler(0, 0, (_player.PlayerMove.PlayerFlip ? 0 : 180));
        // 動く距離と向きの設定
        _dir = new Vector3((_player.PlayerMove.PlayerFlip ? 1 :-1), 0, 0) * _range;
        //DoTweenでの位置変更
        _tw = DOTween.To(() => 0f, x
            => _currentTimer = x,
            Mathf.PI, _timer
            ).OnUpdate(() => transform.transform.localPosition = _dir * Mathf.Sin(_currentTimer) + _originLocalPos)
            .OnComplete(() =>
            {
                this.gameObject.SetActive(false);
                _player.PlayerMove.IsFreeze = false;
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ITeleportable>(out ITeleportable tp))
        {
            Vector3 pos = other.transform.position;
            tp.Teleport(_player.transform.position);
            _parentTp.Teleport(pos);
            this.gameObject.SetActive(false);
            _tw.Complete();
        }
    }
}
