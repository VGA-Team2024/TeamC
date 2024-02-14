using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    /// <summary> BossのSuperクラス </summary>
    public class BossSuperClass : MonoBehaviour, IInitializedTarget
    {
        private decimal _hp = 0; // the hp of boss

        /// <summary> ボスのHPを返す </summary>
        public decimal GetHP
        {
            get { return _hp; }
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

        float CalculateHealthPoint(int clearedStages)
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

            return basedHP + additionHP;
        }

        public void InitObject()
        {
            // calculate hp
            var player = FindFirstObjectByType<PlayerSuperClass>();
            var clearedStage = 0;
            // when IPlayer Is Inherited
            if ((player as IPlayer) != null)
            {
                // Try Getting Cleared Stage
                clearedStage = player.GetClearedStageAmount();
            }
        }
    }
}