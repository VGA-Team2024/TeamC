using UnityEngine;

/// <summary> プレイヤーのMoveを取得する </summary>
interface IPlayerTarget
{
    public void GetPlayerMove(PlayerMove playerMove){}
    public void GetPlayerPos(Vector2 playerPos){}
}
