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
    
    private Tween _tw;
    private float _currentTimer;
    [SerializeField, InspectorVariantName("特殊攻撃の距離")] 
    private float _range = 8;

    private void Awake()
    {
        _pm = transform.parent.gameObject.GetComponent<PlayerMove>();
        _player = transform.parent.gameObject;
        _playerTp = _player.GetComponent<ITeleportable>();
    }

    private void OnEnable()
    {
        _pm.IsFreeze = true;
        _dir = new Vector3((_pm.PlayerFlip ? 1 :-1), 0, 0) * _range;
        _tw = DOTween.To(() => 0f, x
            => _currentTimer = x,
            Mathf.PI, _timer
            ).OnUpdate(() => this.transform.transform.localPosition = _dir * Mathf.Sin(_currentTimer))
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
            _tw.Complete();
            tp.Teleport(_player.transform.position);
            _playerTp.Teleport(other.transform.position);
            this.gameObject.SetActive(false);
        }
    }
}
