using System;
using UnityEngine;

namespace TeamC
{
    /// <summary>戦士の効果</summary>
    [CreateAssetMenu(fileName = "GeneratedWarriorEffects", menuName = "CreateWarriorEffects")]
    public class WarriorEffects : ScriptableObject
    {
        [SerializeField, Header("攻撃のリキャストタイム")] private float recastTime = 10.0f;
        [SerializeField, Header("攻撃のダメージの倍率")] private double magnification = 1.25;

        private Warrior _warrior;
        private Boss _boss;
        private Poet _poet;

        // リキャストタイム用タイマー
        private float _currentRecastTimer = 0;

        /// <summary>戦士の効果発動時の処理</summary>
        public void OnWarriorEffects()
        {
            // 戦士、ボス、詩人の検索
            if (_warrior == null)
                _warrior = FindFirstObjectByType<Warrior>();
            if (_boss == null)
                _boss = FindFirstObjectByType<Boss>();
            if (_poet == null)
                _poet = FindFirstObjectByType<Poet>();

            if (_currentRecastTimer >= recastTime)
            {
                // 10秒に1回1×1.25のレベル-1乗のダメージ
                decimal damage = (decimal)Math.Pow(magnification, _warrior.GetCurrentLevel() - 1);
                _boss.ApplyDamageToBoss(damage);
                // タイマーのリセット
                _currentRecastTimer = 0;

#if UNITY_EDITOR
                Debug.Log($"戦士がボスへ{damage}ダメージ与えた!");
#endif
            }

            _currentRecastTimer += Time.fixedDeltaTime;
        }
    }
}
