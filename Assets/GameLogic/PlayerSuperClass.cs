using System;
using UnityEngine;

namespace TeamC
{
    /// <summary> PlayerのSuperクラス </summary>
    public class PlayerSuperClass : MonoBehaviour, IPlayer
    {
        // Money [G]
        private decimal _currentResource = 1;

        // the amount of stage which cleared
        private int _clearedStageAmount = 0;

        // the damage which player can apply to Game Logic Processing Damage of Boss
        private decimal _applayableDamage = 10;
        public float CalculateApplyingDamageToBoss()
        {
            throw new NotImplementedException();
        }

        public int GetClearedStageAmount()
        {
            throw new NotImplementedException();
        }
    }
}