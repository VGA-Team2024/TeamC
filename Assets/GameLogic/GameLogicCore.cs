using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    /// <summary> ゲーム内のオブジェクトが継承する </summary>
    public interface IInitializedTarget
    {
        /// <summary> ゲーム内のオブジェクトを初期化する </summary>
        public void InitObject();
    }

    /// <summary> ボスが継承する </summary>
    public interface IBoss
    {
    }

    /// <summary> ショップが継承する </summary>
    public interface IShop
    {
    }

    /// <summary> NPCが継承する </summary>
    public interface INonPlayerCharacter
    {
    }

    /// <summary> スキルが継承する </summary>
    public interface ISkills
    {
    }

    /// <summary> プレイヤーが継承する </summary>
    public interface IPlayer
    {
        /// <summary>ボスへのダメージを計算したうえで確定する処理 </summary>
        public float CalculateApplyingDamageToBoss();

        /// <summary> 現在到達したステージ数を返す処理 </summary>
        public int GetClearedStageAmount();
    }

    /// <summary> ゲームロジックの処理を担うクラス </summary>
    public class GameLogicCore : MonoBehaviour
    {
    }
}