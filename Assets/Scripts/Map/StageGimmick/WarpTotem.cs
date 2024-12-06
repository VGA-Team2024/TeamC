using UnityEngine;
[RequireComponent(typeof(BoxCollider))]

public class WarpTotem : MonoBehaviour ,ITeleportable
{
    void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    //このスクリプトが付いているオブジェクトにプレイヤーが特殊攻撃をするとテレポートする
    public void Teleport(Vector3 position)
    {
    }
}
