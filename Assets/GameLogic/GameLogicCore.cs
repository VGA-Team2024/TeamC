using System;
using UnityEngine;

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

        /// <summary> ゲーム内のオブジェクトを終了処理させる </summary>
        public void FinalizeObject();
    }

    /// <summary> ボスのクラスが継承する </summary>
    public interface IBoss
    {
        /// <summary> ボスが死んだときのCallback関数 </summary>
        public Action CallbackOnDeath { get; set; }

        /// <summary> 死んだときにこれを呼ぶ </summary>
        public void OnDeath();

        /// <summary> ボスへダメージを加える時にこれを呼ぶ </summary>
        public void ApplyDamage(float damage);
    }

    /// <summary> ショップが継承する </summary>
    public interface IShop
    {
    }

    /// <summary> NPCが継承する </summary>
    public interface INonPlayerCharacter
    {
    }

    /// <summary> スキルが継承する </summary>
    public interface ISkills
    {
    }

    /// <summary> プレイヤーが継承する </summary>
    public interface IPlayer
    {
        /// <summary>ボスへのダメージを計算したうえで確定する処理PlayerのみGLへのボスへのダメージのアプライを許す </summary>
        public float CalculateApplyingDamageToBoss();

        /// <summary> 現在到達したステージ数を返す処理 </summary>
        public int GetClearedStageAmount();
    }

    public interface IDataSaver
    {
        /// <summary> クライアントのデータを読み込む </summary>
        /// <param name="clientData"></param>
        public void ReadData(ClientDataTemplate clientData);

        /// <summary> クライアントのデータを書き込む </summary>
        public void SaveData(ClientDataTemplate clientData);
    }

    /// <summary> ゲームロジックの処理を担うクラス </summary>
    public class GameLogicCore : MonoBehaviour
    {
        /// on scene transit
        private void OnEnable()
        {
        }

        /// initialize game-objects
        /// { 1.Boss }
        /// <summary> Bossが死んだときの処理 </summary>
        void CalledMethodOnBossDeath()
        {
        }

        /// { 2.NPCs }
        /// { 3.Shop } 
        void Initialize() // Initialize On GameLogic Was Started
        {
            /// Init Boss
            var boss = FindFirstObjectByType<BossSuperClass>();
            boss.CallbackOnDeath += this.CalledMethodOnBossDeath;
        }

        private void Start()
        {
        }

        private void Update()
        {
        }

        private void LateUpdate()
        {
        }

        private void OnDisable()
        {
        }
    }
}