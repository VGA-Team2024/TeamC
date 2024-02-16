using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    /// <summary> Playerのコンポーネント。これがSceneに存在 </summary>
    public class Player : PlayerSuperClass
    {
        /// <summary> 現状のリソース量を取得 </summary>
        public decimal GetCurrentResource() => base.CurrentResource;
    }
}
