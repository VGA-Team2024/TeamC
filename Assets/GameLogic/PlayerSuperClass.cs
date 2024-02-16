using System;
using UnityEngine;

namespace TeamC
{
    /// <summary> PlayerのSuperクラス </summary>
    public class PlayerSuperClass : MonoBehaviour, IPlayer
    {
        // Money [G]
        protected decimal _currentResource = 1;
        
        // damage amount on clicked boss
        protected decimal _damageOnClick = 10;
        
        // the amount of stage which cleared
        protected int _clearedStageAmount = 0;

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