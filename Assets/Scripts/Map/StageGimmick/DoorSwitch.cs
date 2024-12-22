using UnityEngine;
/// <summary>
/// ドアの開閉を管理するクラス
/// </summary>
public enum KeyConditions
{
    DoorOpen,           //プレイヤーが武器で攻撃したらドアが開く
    DoorClose,          //ドアが閉まる    
    MusicBox            //オルゴールを鳴らしたらドアが開く
}

public class DoorSwitch : MonoBehaviour ,IDamageable
{
    [SerializeField,InspectorVariantName("Switch、プレイヤーの攻撃で開く、MusicBox、オルゴールが呼び出されたら開く")] private KeyConditions _keyConditions;
    [SerializeField] GameObject _doorPrefab;
    [SerializeField] Sprite _switchSprite;

    bool _isSwitch = false;
    /// <summary>ドアを開くメソッド</summary>
    void DoorOpen()
    {
        // TODO ドアが開く処理を書く
        _doorPrefab.SetActive(false);
        CRIAudioManager.BGM.Play("SE_Gimmick", "SE_Gimmick_Door01");
        _isSwitch = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = _switchSprite;
    }
    /// <summary>このメソッドが呼ばれたらオブジェクトのアクティブ状態をオンにする</summary>
    void DoorClose()
    {
        _doorPrefab.SetActive(true);
        _isSwitch = true;
    }

    public void TakeDamage(int damage)
    {
        if (!_isSwitch)
        {
            if (_keyConditions == KeyConditions.DoorOpen)
            {
                DoorOpen();
            }
            else if (_keyConditions == KeyConditions.MusicBox)
            {
                // TODO オルゴールの？処理を書く
            }
        }
    }
}
