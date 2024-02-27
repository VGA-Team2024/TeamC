using System.Collections.Generic;
using System;

namespace TeamC
{
    /// <summary> ゲーム内のオブジェクトが継承する </summary>
    public interface IInitializedTarget
    {
        /// <summary> ゲーム内のオブジェクトを初期化する </summary>
        public void InitializeObject();

        /// <summary>
        /// 処理を一時停止する
        /// </summary>
        public void PauseObject();

        /// <summary>
        /// 処理の再開をする
        /// </summary>
        public void ResumeObject();

        /// <summary> ゲーム内のオブジェクトを終了処理させる </summary>
        public void FinalizeObject();
    }

    /// <summary> ボスのクラスが継承する </summary>
    public interface IBoss
    {
        /// <summary> ボスが死んだときのCallback関数GLから初期化される </summary>
        public Action CallbackOnDeath { get; set; }

        /// <summary> ボスへダメージを加える時にこれを呼ぶ </summary>
        public void ApplyDamageToBoss(float damage);

        /// <summary> ボス撃破時のリワードを取得 </summary>
        /// <returns></returns>
        public decimal GetReward();
    }

    /// <summary> ショップが継承する </summary>
    public interface IShop
    {
        /// <summary> NPCのコストの算出 </summary>
        public decimal CalculateCostToBuy(string npcName);

        /// <summary> プレイヤーのゴールドを減らす </summary>
        public void DecreasePlayerSource(string npcName, decimal cost, Action<int, string> taskToInstantiate);
    }

    /// <summary> NPCが継承する </summary>
    public interface INonPlayerCharacter
    {
        /// <summary> NPC名を返す </summary>
        public string GetNPCName();

        /// <summary> ベース価格を返す </summary>
        public float GetBasePrice();

        /// <summary>
        /// NPCが購入されたときに呼び出される。パラメータは購入数
        /// </summary>
        public Action<int> TaskOnShopBoughtCharacter { get; set; }
    }

    /// <summary> スキルが継承する </summary>
    public interface ISkills
    {
        /// <summary> スキルを発動する </summary>
        public void FireSkills();
    }

    /// <summary> 仙人クラスに継承してほしいインターフェイス </summary>
    public interface IHermit
    {
        /// <summary> 発動可能なスキルを返す </summary>
        public List<SkillsDataTemplate> GetFirableSkills();
    }

    /// <summary> プレイヤーが継承する </summary>
    public interface IPlayer
    {
        /// <summary> 現在到達したステージ数を返す処理 </summary>
        public int GetClearedFloorAmount();

        /// <summary> リワード（増えるお金）をプレイヤーに適応する </summary>
        public void ApplyRewardToPlayer(decimal rewards);

        /// <summary> 引数の値分リソースを減らす </summary>
        public void DecreasePlayerGold(decimal amount);
    }

    public interface IDataSaver
    {
        /// <summary> クライアントのデータを読み込む </summary>
        /// <param name="clientData"></param>
        public ClientDataTemplate ReadData();

        /// <summary> クライアントのデータを書き込む </summary>
        public void SaveData();
    }
}