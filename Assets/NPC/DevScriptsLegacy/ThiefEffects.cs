using UnityEngine;

namespace TeamC
{
    /// <summary>盗賊の効果</summary>
    [CreateAssetMenu(fileName = "GeneratedThiefEffects", menuName = "CreateThiefEffects")]
    public class ThiefEffects : ScriptableObject
    {
        [SerializeField, Header("盗みのリキャストタイム")] private float recastTime = 10.0f;
        [SerializeField, Header("盗みの報酬の割る数")] private double magnification = 100000;

        private Thief _thief;
        private BossClass _bossLagacy;
        private PlayerL _playerL;
        private Poet _poet;

        // リキャストタイム用タイマー
        private float _currentRecastTimer;

        /// <summary>盗賊の効果発動時の処理</summary>
        public void OnThiefEffects()
        {
            // 盗賊、ボス、プレイヤー、詩人の検索
            if (_thief == null)
                _thief = FindFirstObjectByType<Thief>();
            if (_bossLagacy == null)
                _bossLagacy = FindFirstObjectByType<BossClass>();
            if (_playerL == null)
                _playerL = FindFirstObjectByType<PlayerL>();
            if (_poet == null)
                _poet = FindFirstObjectByType<Poet>();

            // リキャストタイムが終われば
            if (_currentRecastTimer >= recastTime)
            {
                // 10秒に1回ボスから獲得できるゴールドの1/10000のレベル倍を入手
                decimal rewards = GameObject.FindFirstObjectByType<GameLogic>().ReturnReward() / (decimal)magnification * _thief.GetCurrentLevel() *
                                  _poet.GetEffectMagnification();
                _playerL.ApplyRewardToPlayer(rewards);
                // タイマーのリセット
                _currentRecastTimer = 0;
                
#if UNITY_EDITOR
                Debug.Log($"盗賊が{rewards}G獲得!");
#endif
            }

            _currentRecastTimer += Time.fixedDeltaTime;
        }
    }
}

