using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public class PlayerL : PlayerSuperClassL
    {
        [SerializeField] private TMP_Text _goldDispLabel;
        [SerializeField] private TMP_Text _floorLabel;
        
        /// <summary> 現状のリソース量を取得 </summary>
        public decimal GetCurrentGold() => base._currentGold;
        
        /// <summary> 現状のクリック時のダメージ量を取得 </summary>
        public decimal GetPlayerApplayingDamage => base._damageOnClick;

        /// <summary> クリック時のダメージ量を引数の値で初期化する </summary>
        public void SetPlayerApplayingDamage(decimal dmg) => base.SetDamageOnClick = dmg;

        public void ApplyDamageToBoss() =>
            GameObject.FindFirstObjectByType<GameLogicCore>().ApplyDamageToBoss(base._damageOnClick);

        /// <summary>
        /// プレイヤーがボス撃破時にこれを呼び出す
        /// </summary>
        public void SendMessagePlayerHadWin() => ++base._clearedFloorAmount;
        
        private void Start()
        {
            // set this player tag
            this.gameObject.tag = "Player";
        }

        private void Update()
        {
            _goldDispLabel.text = "賞金 : " + GetCurrentGold().ToString("N0");
            _floorLabel.text = "階層 : " + _clearedFloorAmount.ToString();
        }
    }
}
