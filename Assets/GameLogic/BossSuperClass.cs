using System;
using UnityEngine;

namespace TeamC
{
    /// <summary> BossのSuperクラス </summary>
    public class BossSuperClass : MonoBehaviour, IInitializedTarget, IBoss
    {
        /// *MEMO*
        /// ～目標～
        /// ボスの派生クラスはUIに対する制御とHPに対する
        /// 制御だけ書けばよいとこまでがSuperクラスで実装できている部分とする
        ///
        /// ～指示～
        /// 
        
        private decimal _hp = 0; // the hp of boss
        private decimal _rewards = 0; // the gold of boss

        /// <summary> ボスのHPを返す </summary>
        public decimal GetHP
        {
            get { return _hp; }
        }

        #region BossInitialization

        decimal CalculateRewards(int clearedStages)
        {
            int baseStage = -1;

            #region CalculateBaseStage

            // baseFloor = 1
            if (1 <= clearedStages && clearedStages < 6)
            {
                baseStage = 1;
            }
            // bF = 6
            else if (6 <= clearedStages && clearedStages < 11)
            {
                baseStage = 6;
            }
            // bF = 11
            else if (11 <= clearedStages && clearedStages < 16)
            {
                baseStage = 11;
            }
            // bF = 16
            else if (16 <= clearedStages && clearedStages < 21)
            {
                baseStage = 16;
            }
            // bF = 21
            else if (21 <= clearedStages && clearedStages < 26)
            {
                baseStage = 21;
            }
            // bF = 26
            else
            {
                baseStage = 26;
            }

            #endregion

            switch (baseStage)
            {
                    // ここの仕様はちょっと細かくなかったためこれでよいか聞いてみたい
                case 1:
                    return 10000 * clearedStages;
                case 6:
                    return 100000 * clearedStages;
                case 11:
                    return 500000 * 11;
                case 16:
                    return 10000000 * 16;
                case 21:
                    return 5000000000 * 21;
                case 26:
                    return 1000000000000 * 26;
                default:
                    return 10000;
            }
        }

        int CalculateBaseHP(int baseStage)
        {
            int basedHP = 0;

            #region CalculateBaseHP

            basedHP = baseStage switch
            {
                // baseFloor = 
                1 => 100,
                6 => 1000,
                11 => 100000,
                16 => 250000,
                21 => 50000000,
                26 => 1000000000,
                _ => 100
            };
            return basedHP;

            #endregion
        }

        float CalculateAdditionHP(int baseStage, int clearedStage)
        {
            float additionHP = 0;
            switch (baseStage)
            {
                case 1:
                    for (int i = 0; i < clearedStage - baseStage; i++)
                        additionHP += 100;
                    break;
                case 6:
                    for (int i = 0; i < clearedStage - baseStage; i++)
                        additionHP *= 1.5f;
                    break;
                case 11:
                case 21:
                case 16:
                    for (int i = 0; i < clearedStage - baseStage; i++)
                        additionHP *= 2.0f;
                    break;
                case 26:
                    for (int i = 0; i < clearedStage - baseStage; i++)
                        additionHP *= 10.0f;
                    break;
            }

            return additionHP;
        }

        decimal CalculateHealthPoint(int clearedStages)
        {
            float basedHP = -1;
            float additionHP = -1;
            int baseStage = -1;

            #region CalculateBaseStage

            // baseFloor = 1
            if (1 <= clearedStages && clearedStages < 6)
            {
                baseStage = 1;
            }
            // bF = 6
            else if (6 <= clearedStages && clearedStages < 11)
            {
                baseStage = 6;
            }
            // bF = 11
            else if (11 <= clearedStages && clearedStages < 16)
            {
                baseStage = 11;
            }
            // bF = 16
            else if (16 <= clearedStages && clearedStages < 21)
            {
                baseStage = 16;
            }
            // bF = 21
            else if (21 <= clearedStages && clearedStages < 26)
            {
                baseStage = 21;
            }
            // bF = 26
            else
            {
                baseStage = 26;
            }

            #endregion

            basedHP = CalculateBaseHP(baseStage);
            additionHP = CalculateAdditionHP(baseStage, clearedStages);

            return (decimal)(basedHP + additionHP);
        }

        #endregion

        public Action CallbackOnDeath { get; set; } // the action given from game logic

        public void OnDeath() // on death call this action
        {
            CallbackOnDeath();
        }

        public void InitObject() // Called On GL-Start
        {
            // calculate hp
            var player = FindFirstObjectByType<PlayerSuperClass>();
            var clearedStage = 0;

            // when IPlayer Is Inherited
            if ((player as IPlayer) != null)
            {
                // Try Getting Cleared Stage
                clearedStage = player.GetClearedStageAmount();
                _hp = CalculateHealthPoint(clearedStage);
                _rewards = CalculateRewards(clearedStage);
            }
        }

        public void FinalObject() // Called On GL-End
        {
            throw new NotImplementedException();
        }
    }
}