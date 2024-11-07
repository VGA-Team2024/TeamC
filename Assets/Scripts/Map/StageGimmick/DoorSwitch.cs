using UnityEngine;
/// <summary>
/// ドアの開閉を管理するクラス
/// </summary>
public enum KeyConditions
{
    Switch,             //プレイヤーが武器で攻撃したらドアが開く
    MusicBox            //オルゴールを鳴らしたらドアが開く
}

public class DoorSwitch : MonoBehaviour
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

    void OnCollisionEnter(Collision other)
    {
        if (_keyConditions == KeyConditions.Switch)
        {
            
            //AttackColliderの名前が付いたオブジェクトに触れたらドアを開く
            if (other.gameObject.CompareTag("Enemy"))
            {
                DoorOpen();
            }
        }
        else if (_keyConditions == KeyConditions.MusicBox)
        {
            //オルゴールの？処理を書く
        }
    }
}
