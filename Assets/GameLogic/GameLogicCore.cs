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
        public void InitObject();

        /// <summary> ゲーム内のオブジェクトを終了処理させる </summary>
        public void FinalObject();
    }

    /// <summary> ボスのクラスが継承する </summary>
    public interface IBoss
    {
        /// <summary> ボスが死んだときのCallback関数 </summary>
        public Action CallbackOnDeath { get; set; }

        /// <summary> 死んだときにこれを呼ぶ </summary>
        public void OnDeath();
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
        /// <summary>ボスへのダメージを計算したうえで確定する処理 </summary>
        public float CalculateApplyingDamageToBoss();

        /// <summary> 現在到達したステージ数を返す処理 </summary>
        public int GetClearedStageAmount();
    }

    public interface IDataSaver
    {
        public void ReadData();
        public void SaveData();
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
        /// <summary> Bossが死んだときにこれを呼び出す </summary>
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