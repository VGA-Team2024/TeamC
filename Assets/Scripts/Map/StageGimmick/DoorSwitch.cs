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
    [SerializeField] KeyConditions _keyConditions;
    [SerializeField,InspectorVariantName("trueの時、ドアが開く")] bool _doorOpen = false;
    
    void Start()
    {
        Debug.Log(_keyConditions);
    }
    
    /// <summary>
    /// ドアを開くメソッド
    /// </summary>
    void DoorOpen()
    {
        Debug.Log("ドアが開く");
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
