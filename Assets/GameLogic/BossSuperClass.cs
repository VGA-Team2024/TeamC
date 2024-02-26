using System;
using UnityEngine;

// 2/16 90% - implemented

namespace TeamC
{
    /// <summary> BossのSuperクラス </summary>
    public class BossSuperClass : MonoBehaviour, IInitializedTarget, IBoss
    {
        // *MEMO*
        // ～目標～
        // ボスの派生クラスはUIに対する制御とHPに対する
        // 制御だけ書けばよいとこまでがSuperクラスで実装できている部分とする

        [SerializeField, Header("The Health Point The Boss Have")]
        private decimal hp = 0; // the hp of boss

        [SerializeField, Header("The Rewards When Defeated The Boss")]
        private decimal rewards = 0; // the gold of boss

        private decimal _maxHpAtCurrentFloor;

        /// <summary> ボスのHPを返す </summary>
        public decimal GetHP
        {
            get { return hp; }
        }

        /// <summary>
        /// ボスのその階層のHPの最大値
        /// </summary>
        public decimal GetMaxHPAtCurrentFloor
        {
            get { return _maxHpAtCurrentFloor; }
        }

        #region BossInitialization

        decimal CalculateRewards(int clearedStages)
        {
            int baseStage = -1;

            #region CalculateBaseStage

            // baseFloor = 1
            if (0 <= clearedStages && clearedStages < 6)
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
            if (0 <= clearedStages && clearedStages < 6)
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

        public decimal GetReward() => rewards;

        /// <summary> ボスが死んだときにこれを呼ぶ </summary>
        // on death call this action
        protected void OnDeath() => CallbackOnDeath();

        // on applied damage
        public void ApplyDamageToBoss(float damage) => hp -= (decimal)damage;
        public void ApplyDamageToBoss(decimal damage) => hp -= damage;

        public void InitializeObject() // Called On GL-Start
        {
            // ★if client data is exist read saved client data★

            // calculate hp
            var player = FindFirstObjectByType<PlayerSuperClass>();
            var clearedStage = 0;
            // Get Cleared Stage
            clearedStage = player.GetClearedFloorAmount();
            hp = CalculateHealthPoint(clearedStage + 1);
            rewards = CalculateRewards(clearedStage + 1);
            _maxHpAtCurrentFloor = hp;
        }

        public void PauseObject()
        {
            throw new NotImplementedException();
        }

        public void ResumeObject()
        {
            throw new NotImplementedException();
        }

        public void FinalizeObject() // Called On GL-End
        {
            // ★save boss hp to client data★ 

            throw new NotImplementedException();
        }
    }
}