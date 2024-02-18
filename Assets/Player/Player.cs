using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    /// <summary> Playerのコンポーネント。これがSceneに存在 </summary>
    public class Player : PlayerSuperClass
    {
        /// <summary> 現状のリソース量を取得 </summary>
        public decimal GetCurrentResource() => base._currentResource;

        /// <summary> 現状のクリック時のダメージ量を取得 </summary>
        public decimal GetPlayerApplayingDamage => base._damageOnClick;

        /// <summary> クリック時のダメージ量を初期化 </summary>
        public void SetPlayerApplayingDamage(decimal dmg) => base.SetDamageOnClick = dmg;
    }
}
