using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    public class Player : PlayerSuperClass
    {
        /// <summary> 現状のリソース量を取得 </summary>
        public decimal GetCurrentResource() => base.CurrentResource;

        /// <summary> 現状の突破ステージ数を取得 </summary>
        public int GetClearedStages() => base.GetClearedStageAmount();
    }
}
