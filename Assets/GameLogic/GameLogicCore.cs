using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SgLibUnite.Singleton;

namespace TeamC
{
    /// *MEMO*
    /// ～目標～
    /// 各コンポーネントとの緊密
    /// （メソッドの呼び合いとかCallBackはGLとSuperクラスで実装している部分）
    /// な部分をあらかた開発＋実装できるとこまでが目標
    /// ～セーブデータの必要なデータ～
    /// プレイヤーの到達したステージ数
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

    /// <summary> ゲームロジックの処理を担うクラス </summary>
    public class GameLogicCore : SingletonBaseClass<GameLogicCore>
    {
        private float _elapsedTime = 0f;
        private ClientDataTemplate savedData = new();
        private bool _isBossDeath = false;

        private void OnEnable()
        {
            // read saved client data and initialize
            var clientSaveDatas = FindFirstObjectByType<ClientDataSaverSuperClass>();
            savedData = clientSaveDatas.ReadData();
        }

        private void MoveToNextFloor()
        {
            var player = FindFirstObjectByType<Player>();
            player.SendMessagePlayerHadWin();
            GameObject.FindFirstObjectByType<Boss>().InitializeObject();
            _isBossDeath = false;
        }

        /// initialize game-objects
        /// { 1.Boss }
        /// <summary> Bossが死んだときの処理 </summary>
        void CalledMethodOnBossDeath()
        {
            if (!_isBossDeath)
            {
                // get reward from boss
                var boss = FindFirstObjectByType<BossSuperClass>();
                var rewards = boss.GetReward();
                // apply reward to player
                var player = FindFirstObjectByType<Player>();
                player.ApplyRewardToPlayer(rewards);
                Debug.Log("Applied Reward");
                Debug.Log($"{rewards.ToString("N0")}");
                _isBossDeath = true;
                MoveToNextFloor();
            }
        }

        /// { 2.NPCs }
        /// { 3.Shop } 
        void Initialize() // Initialize On GameLogic Was Started
        {
            /// Init Boss
            var boss = FindFirstObjectByType<BossSuperClass>();
            boss.CallbackOnDeath += this.CalledMethodOnBossDeath;

            // init all GOs
            var gos = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            // filtering inherited class IInitializedTarget
            var initTargets = gos.ToList().Where(_ => _.GetComponent<IInitializedTarget>() != null).ToList();
            foreach (var obj in initTargets)
            {
                obj.GetComponent<IInitializedTarget>().InitializeObject();
            } // initialize all objects
        }

        /// { 4.Player + NPC}
        public void ApplyDamageToBoss(decimal damage)
        {
            var boss = GameObject.FindFirstObjectByType<BossSuperClass>();
            boss.ApplyDamageToBoss(damage);
        }

        protected override void ToDoAtAwakeSingleton()
        {
        }

        private void Start()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            _elapsedTime = Time.deltaTime;

            if (_elapsedTime >= 60.0f)
            {
                // save client data
                var clientSaveDatas = FindFirstObjectByType<ClientDataSaverSuperClass>();
                clientSaveDatas.SaveData();
                _elapsedTime = 0f;
            } // if elapsed one minutes
        }

        private void OnDisable()
        {
            // save client data
            var clientSaveDatas = FindFirstObjectByType<ClientDataSaverSuperClass>();
            clientSaveDatas.SaveData();

            // finalize all GOs
            var gos = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            // filtering inherited class IInitializedTarget
            var initTargets = gos.ToList().Where(_ => _.GetComponent<IInitializedTarget>() != null).ToList();
            foreach (var obj in initTargets)
            {
                obj.GetComponent<IInitializedTarget>().FinalizeObject();
            } // finalize all objects
        }
    }
}