using UnityEngine.Events;

namespace TeamC
{
    public interface IBossClass
    {
        /// <summary>
        /// 現在のボスのHPを返す
        /// </summary>
        public decimal ReturnCurrentBossHP { get; }

        /// <summary>
        /// ボスの現在の階層の最大HPを返す
        /// </summary>
        public decimal ReturnMaxBossHP { get; }

        /// <summary>
        /// ボスへダメージを与える
        /// </summary>
        /// <param name="dmg"></param>
        public void AddDamageToBoss(float dmg);

        /// <summary>
        /// ボスへダメージを与える
        /// </summary>
        /// <param name="dmg"></param>
        public void AddDamageToBoss(decimal dmg);
    }

    public interface IGameLogic
    {
        /// <summary>
        /// リワードを返す
        /// </summary>
        /// <returns></returns>
        public decimal ReturnReward();
    }

    public interface IClientData
    {
        public decimal BossHP { get; set; }

        public int CurrentFloor { get; set; }

        public decimal CurrentGold { get; set; }
    }

    public interface INPC
    {
        public bool IsActive { get; }
        
        public UnityEvent GetNPCEffect { get; }
    }

    public interface IPlayer
    {
        
    }
}