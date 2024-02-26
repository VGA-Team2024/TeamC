using System;
using UnityEngine;

namespace TeamC
{
    /// <summary> PlayerのSuperクラス </summary>
    public class PlayerSuperClass : MonoBehaviour, IPlayer
    {
        // base dmg 
        private const decimal PLAYERBASEDMG = 10;

        // Money [G]
        protected decimal _currentGold = 1;

        // damage amount on clicked boss
        protected decimal _damageOnClick = 10;
        
        /// <summary> ダメージ量を初期化する </summary>
        protected decimal SetDamageOnClick
        {
            set { _damageOnClick = value; }
        }

        // the amount of stage which cleared
        protected int _clearedFloorAmount = 0;
        
        public int GetClearedFloorAmount() // return stage cleared
        {
            return _clearedFloorAmount;
        }

        public void ApplyRewardToPlayer(decimal rewards) // apply rewards
        {
            _currentGold += rewards;
            Debug.Log($"Current Gold : {_currentGold}");
        }

        public void DecreasePlayerGold(decimal amount) // apply reduce resource to player
        {
            _currentGold -= amount;
        }
    }
}