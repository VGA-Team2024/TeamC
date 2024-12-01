
using UnityEngine;
/// <summary>
/// 吹き飛ぶオブジェクトに付ける
/// </summary>
public interface IBlowable
{
    
    /// <summary>
    /// 吹き飛ばされる側の持つ関数
    /// </summary>
    /// <param name="dir">攻撃"する側"のPosition</param>
    public void BlownAway(Vector3 pos);
}
