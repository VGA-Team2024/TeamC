using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    #region memo

    // ＠取得できる値＠
    // ゴールド
    // ダメージ/クリック
    // 突破ステージ数（基底クラス）

    #endregion
    /// <summary> Playerのコンポーネント。これがSceneに存在 </summary>
    public class Player : PlayerSuperClass
    {
        /// <summary> 現状のリソース量を取得 </summary>
        public decimal GetCurrentGold() => base._currentGold;
        
        /// <summary> 現状のクリック時のダメージ量を取得 </summary>
        public decimal GetPlayerApplayingDamage => base._damageOnClick;

        /// <summary> クリック時のダメージ量を初期化 </summary>
        public void SetPlayerApplayingDamage(decimal dmg) => base.SetDamageOnClick = dmg;

        private void Start()
        {
            // set this player tag
            this.gameObject.tag = "Player";
        }
    }
}
