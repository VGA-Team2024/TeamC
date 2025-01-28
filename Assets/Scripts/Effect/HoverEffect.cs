using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    [SerializeField, InspectorVariantName("fairyAddableの値")]
    private int _fairyAddableValue = 1;

    [SerializeField, InspectorVariantName("自身の移動スピード")]
    float _effectSpeed = 2;

    private GameObject _player;

    [SerializeField]
    bool _ismove = true;

    SpriteRenderer fairy;

    TrailRenderer trail;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        fairy = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        MoveHoverEffect();
    }

    /// <summary>
    /// 自身をプレイヤーの方向に移動させるメソッド
    /// </summary>
    private void MoveHoverEffect()
    {
        //プレイヤーに向かう処理
        transform.position =
            Vector3.MoveTowards(transform.position, _player.transform.position
                , _effectSpeed * Time.deltaTime);

        if (Vector3.Distance(gameObject.transform.position, _player.transform.position) <= 0.1)
        {
            if (_player.TryGetComponent<IFairyAddable>(out IFairyAddable fairyAddable))
            {
                fairyAddable.AddFairy(_fairyAddableValue);
            }
            fairy.color = new Color(fairy.color.r, fairy.color.g, fairy.color.b, 0);
            _ismove = false;
            trail.time -= 0.02f;
            if (trail.time < 0)
            {
                Destroy(gameObject);
            }
        }

    }
}