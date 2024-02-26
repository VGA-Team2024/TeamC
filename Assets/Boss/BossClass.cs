using UnityEngine;

namespace TeamC
{
    public class BossClass : MonoBehaviour, IBossClass
    {
        [SerializeField, Header("The Health Point The Boss Have")]
        private decimal _health = 0; // the hp of boss

        private decimal _maxHpAtCurrentFloor;

        public decimal ReturnCurrentBossHP
        {
            get { return this._health; }
        }

        public decimal ReturnMaxBossHP
        {
            get { return this._maxHpAtCurrentFloor; }
        }

        public void AddDamageToBoss(float dmg)
        {
            this._health -= (decimal)dmg;
        }

        public void AddDamageToBoss(decimal dmg)
        {
            this._health -= dmg;
        }

        #region PreparingProcess

        int CalculateBaseFloorByClearedFloor(int clearedFloor)
        {
            int baseStage = 0;

            // baseFloor = 1
            if (0 <= clearedFloor && clearedFloor < 6)
            {
                baseStage = 1;
            }
            // bF = 6
            else if (6 <= clearedFloor && clearedFloor < 11)
            {
                baseStage = 6;
            }
            // bF = 11
            else if (11 <= clearedFloor && clearedFloor < 16)
            {
                baseStage = 11;
            }
            // bF = 16
            else if (16 <= clearedFloor && clearedFloor < 21)
            {
                baseStage = 16;
            }
            // bF = 21
            else if (21 <= clearedFloor && clearedFloor < 26)
            {
                baseStage = 21;
            }
            // bF = 26
            else
            {
                baseStage = 26;
            }

            return baseStage;
        }

        int CalculateBaseHP(int baseFloor)
        {
            int basedHP = 0;
            basedHP = baseFloor switch
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
        }

        float CalculateAdditionHP(int baseFloor, int clearedFloor)
        {
            float additionHP = 0;
            switch (baseFloor)
            {
                case 1:
                    for (int i = 0; i < clearedFloor - baseFloor; i++)
                        additionHP += 100;
                    break;
                case 6:
                    for (int i = 0; i < clearedFloor - baseFloor; i++)
                        additionHP *= 1.5f;
                    break;
                case 11:
                case 21:
                case 16:
                    for (int i = 0; i < clearedFloor - baseFloor; i++)
                        additionHP *= 2.0f;
                    break;
                case 26:
                    for (int i = 0; i < clearedFloor - baseFloor; i++)
                        additionHP *= 10.0f;
                    break;
            }

            return additionHP;
        }

        decimal CalculateHealthPoint(int clearedFloor)
        {
            float basedHP = -1;
            float additionHP = -1;
            int baseStage = -1;

            baseStage = CalculateBaseFloorByClearedFloor(clearedFloor);

            basedHP = CalculateBaseHP(baseStage);
            additionHP = CalculateAdditionHP(baseStage, clearedFloor);

            return (decimal)(basedHP + additionHP);
        }

        #endregion
    }
}