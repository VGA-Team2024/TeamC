using UnityEngine;
/// <summary>
/// ドアの開閉を管理するクラス
/// </summary>
public enum KeyConditions
{
    Switch,             //プレイヤーが武器で攻撃したらドアが開く
    MusicBox            //オルゴールを鳴らしたらドアが開く
}

public class DoorSwitch : MonoBehaviour ,IDamageable
{
    [SerializeField,InspectorVariantName("Switch、プレイヤーの攻撃で開く、MusicBox、オルゴールが呼び出されたら開く")] private KeyConditions _keyConditions;
    [SerializeField] GameObject _doorPrefab;
    /// <summary>ドアを開くメソッド</summary>
    void DoorOpen()
    {
        //ドアが開く処理を書く
        _doorPrefab.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (_keyConditions == KeyConditions.Switch)
        {
            DoorOpen();
        }
        else if (_keyConditions == KeyConditions.MusicBox)
        {
            //オルゴールの？処理を書く
        }
    }
}
