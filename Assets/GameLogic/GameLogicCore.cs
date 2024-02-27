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