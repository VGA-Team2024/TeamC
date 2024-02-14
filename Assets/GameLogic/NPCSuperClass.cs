using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    /// <summary> NPCのSuperクラス </summary>
    public class NPCSuperClass : MonoBehaviour
    {
        private int _currentLv = 1;         // level
        private float _basePrice = 1f;      // price
        private float _currentDmg = 1f;     // damage

        /// <summary> levelを取得する </summary>
        public int GetLevel
        {
            get { return _currentLv; }
        }

        /// <summary> levelの値を初期化する </summary>
        public int SetLevel
        {
            set { _currentLv = value; }
        }
    }
}