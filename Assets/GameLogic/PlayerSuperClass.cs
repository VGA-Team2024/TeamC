using System;
using UnityEngine;

namespace TeamC
{
    /// <summary> PlayerのSuperクラス </summary>
    public class PlayerSuperClass : MonoBehaviour, IPlayer
    {
        // Money [G]
        private decimal _currentResource = 1;

        /// <summary> 現状のリソース量 </summary>
        protected decimal CurrentResource
        {
            get { return _currentResource; }
            set { _currentResource = value; }
        }

        // the amount of stage which cleared
        private int _clearedStageAmount = 0;

        // the damage which player can apply to Game Logic Processing Damage of Boss
        private decimal _appliableDamage = 10;

        /// <summary> ボスへ割り当てるダメージ総量 </summary>
        protected decimal AppliableDamage
        {
            get { return _appliableDamage; }
            set { _appliableDamage = value; }
        }

        public int GetClearedStageAmount() // return stage cleared
        {
            return _clearedStageAmount;
        }

        public void ApplyRewardToPlayer(decimal rewards) // apply rewards
        {
            _currentResource += rewards;
        }

        protected void ApplyDamageToBoss()
        {
            var gl = GameObject.FindFirstObjectByType<GameLogicCore>();
            gl.ApplyDamageToBoss(_appliableDamage);
        }
    }
}