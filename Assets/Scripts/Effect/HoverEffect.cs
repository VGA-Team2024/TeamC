using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    [SerializeField,InspectorVariantName("fairyAddableの値")] 
    private int _fairyAddableValue = 1;

    [SerializeField, InspectorVariantName("自身の移動スピード")]
    float _effectSpeed = 2;
    
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
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

            Destroy(gameObject);
        }
    }
}