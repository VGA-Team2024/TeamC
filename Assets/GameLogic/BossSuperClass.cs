using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    /// <summary> BossのSuperクラス </summary>
    public class BossSuperClass : MonoBehaviour
    {
        private decimal _hp = 0;
        /// <summary> ボスのHPを返す </summary>
        public decimal GetHP
        {
            get { return _hp; }
        }
    }
}
