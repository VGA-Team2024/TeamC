using UnityEngine;

public class ObjectTeleport : MonoBehaviour ,ITeleportable
{
    /// <summary>
    /// 移動するスクリプト
    /// </summary>
    /// <param name="position"></param>
    public void Teleport(Vector3 position)
    {
        transform.position = position;
        CRIAudioManager.BGM.Play("SE_Gimmick", "SE_Gimmick_WarpTotem");
    }
}
