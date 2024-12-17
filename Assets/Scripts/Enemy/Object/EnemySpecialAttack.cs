using UnityEngine;
using DG.Tweening;

public class EnemySpecialAttack : MonoBehaviour
{
    private ITeleportable _parentTp;
    private Vector3 _dir;
    [SerializeField,InspectorVariantName("進み切るの時間")] 
    private float _advanceTimer = 1;
    [SerializeField, InspectorVariantName("進む時のイージング")]
    Ease _advanceEase = Ease.Linear;
    [SerializeField,InspectorVariantName("戻るまでの時間")] 
    private float _returnTimer = 1;
    [SerializeField, InspectorVariantName("戻る時のイージング")]
    Ease _returnEase = Ease.Linear;

    private Nancy _nancy;
    private Vector3 _originLocalPos;
    
    private Tween _twForward;
    private Tween _twBack;
    private float _currentPos;
    [SerializeField, InspectorVariantName("特殊攻撃の距離")] 
    private float _range = 8;
    [SerializeField, InspectorVariantName("Hit時ずらしVec3")]
    private Vector3 _hitMisalignment = new Vector3(0f, 0.5f, 0f);

    private void Awake()
    {
        _parentTp = transform.parent.gameObject.GetComponent<ITeleportable>();
        _originLocalPos = transform.localPosition;
        _nancy = transform.parent.gameObject.GetComponent<Nancy>();
    }

    private void OnEnable()
    {
        //音の再生
        // 針の向きの見た目の変更
        // transform.localRotation = Quaternion.Euler(0, 0, (_player.PlayerMove.PlayerFlip ? 0 : 180));
        // 動く距離と向きの設定
        // _dir = new Vector3((_player.PlayerMove.PlayerFlip ? 1 :-1), 0, 0) * _range;
        _dir = Vector3.right * _range;
        //DoTweenでの位置変更
        NeedleForwardMove();
    }

    private void OnDisable()
    {
        _twForward.Kill();
        _twBack.Kill();
    }

    void NeedleForwardMove()
    {
        //DoTweenでの位置変更
        _twForward = DOTween.To(() => 0f, x
                    => _currentPos = x,
                _range, _advanceTimer
            ).OnUpdate(() => transform.transform.localPosition = _dir * _currentPos + _originLocalPos)
            .OnComplete(() => NeedleBackMove(_range,_returnTimer)).SetEase(_advanceEase);
    }

    void NeedleBackMove(float backRange , float time)
    {
        _twBack = DOTween.To(() => backRange, x
                    => _currentPos = x,
                0, time
            ).OnUpdate(() => transform.transform.localPosition = _dir * _currentPos + _originLocalPos)
            .OnComplete(() => 
            {
                gameObject.SetActive(false);
            }).SetEase(_returnEase);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ITeleportable>(out ITeleportable tp))
        {// テレポート対象に当たる
            Vector3 pos = other.transform.position;
            tp.Teleport(_nancy.transform.position + _hitMisalignment);
            _parentTp.Teleport(pos + _hitMisalignment);
            _twForward.Kill(false);
            gameObject.SetActive(false);
        }
        else if(other.isTrigger == false)
        {// 対象外のオブジェクに当たる
            _twForward.Kill(false);
            NeedleBackMove(_currentPos,_currentPos / _range * _returnTimer);
        }
    }
}
