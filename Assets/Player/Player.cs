using TMPro;
using UnityEngine;

namespace TeamC
{
    #region memo

    // ＠取得できる値＠
    // ゴールド
    // ダメージ/クリック
    // 突破ステージ数（基底クラス）

    #endregion

    /// <summary> Playerのコンポーネント。これがSceneに存在 </summary>
    public class Player : MonoBehaviour, IPlayer, IInitializedTarget
    {
        [SerializeField] private TMP_Text _goldDispLabel;
        [SerializeField] private TMP_Text _floorLabel;

        // Money [G]
        private decimal _currentGold;

        // damage amount on clicked boss
        private decimal _damageOnClick;

        // the amount of stage which cleared
        private int _clearedFloorAmount;

        public int GetClearedFloorAmount() // return stage cleared
        {
            return _clearedFloorAmount;
        }

        public void ApplyRewardToPlayer(decimal rewards) // apply rewards
        {
            _currentGold += rewards;
            Debug.Log($"Current Gold : {_currentGold}");
        }

        public void DecreasePlayerGold(decimal amount) // apply reduce resource to player
        {
            _currentGold -= amount;
        }

        /// <summary> 現状のリソース量を取得 </summary>
        public decimal GetCurrentGold() => _currentGold;

        /// <summary> 現状のクリック時のダメージ量を取得 </summary>
        public decimal GetPlayerApplayingDamage => _damageOnClick;

        /// <summary> クリック時のダメージ量を引数の値で初期化する </summary>
        public void SetPlayerApplayingDamage(decimal dmg) => _damageOnClick = dmg;

        public void ApplyDamageToBoss()
        {
            GameObject.FindFirstObjectByType<GameLogicCore>().ApplyDamageToBoss(_damageOnClick);
            Debug.Log($"Player DMG:{_damageOnClick.ToString("N0")}");
        }

        /// <summary>
        /// プレイヤーがボス撃破時にこれを呼び出す
        /// </summary>
        public void SendMessagePlayerHadWin() => ++_clearedFloorAmount;

        private void Start()
        {
            // set this player tag
            this.gameObject.tag = "Player";
        }

        private void Update()
        {
            _goldDispLabel.text = "賞金 : " + GetCurrentGold().ToString("N0");
            _floorLabel.text = "階層 : " + (_clearedFloorAmount + 1).ToString();
        }

        public void InitializeObject()
        {
            // Money [G]
            _currentGold = 1;

            // damage amount on clicked boss
            _damageOnClick = 10;

            // the amount of stage which cleared
            _clearedFloorAmount = 0;
        }

        public void PauseObject()
        {
            throw new System.NotImplementedException();
        }

        public void ResumeObject()
        {
            throw new System.NotImplementedException();
        }

        public void FinalizeObject()
        {
        }
    }
}