using UnityEngine;


/// <summary>
/// テレポート可能なオブジェクトに付けるインターフェース
/// </summary>
public interface ITeleportable
{
    /// <summary>
    /// テレポートされる側に付ける関数
    /// </summary>
    /// <param name="position">テレポート先</param>
    public void Teleport(Vector3 position);
}
