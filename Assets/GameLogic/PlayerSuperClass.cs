using System;
using UnityEngine;

namespace TeamC
{
    /// <summary> PlayerのSuperクラス </summary>
    public class PlayerSuperClass : MonoBehaviour, IPlayer
    {
        // Money [G]
        private decimal _currentResource = 1;
        
        // damage amount on clicked boss
        private decimal _damageOnClick = 10;

        /// <summary> 現状のリソース量 </summary>
        protected decimal CurrentResource
        {
            get { return _currentResource; }
            set { _currentResource = value; }
        }

        /// <summary> クリック時のダメージ量 </summary>
        protected decimal CurrentDamagesOnClick
        {
            get { return _damageOnClick; }
            set { _damageOnClick = value; }
        }
        
        protected 

        // the amount of stage which cleared
        private int _clearedStageAmount = 0;

        public int GetClearedStageAmount() // return stage cleared
        {
            return _clearedStageAmount;
        }

        public void ApplyRewardToPlayer(decimal rewards) // apply rewards
        {
            _currentResource += rewards;
        }

        public void DecreasePlayerResource(decimal amount) // apply reduce resource to player
        {
            _currentResource -= amount;
        }
    }
}