using UnityEngine;

namespace TeamC
{
    /// <summary>戦士の処理</summary>
    public class Warrior : NPCSuperClass
    {
        [SerializeField, Header("攻撃のリキャストタイム")] private float recastTime = 10.0f;
        [SerializeField, Header("攻撃のダメージの倍率")] private double magnification = 1.25;
        
        private float _currentRecastTimeCount;

        void OnEnable()
        {
            // 雇用してたらキャラクターを表示する
            // タイマーの初期化
            _currentRecastTimeCount = 0;
        }

        private void FixedUpdate()
        {
            if (_currentRecastTimeCount >= recastTime)
            {
                Debug.LogWarning("未実装です");
                return;
                // 10秒に1回1×1.25のレベル-1乗のダメージ
                // decimal BossHP = 10;
                // ulong Level = 1;
                // ulong PoetBuff = 1;
                // BossHP -= (decimal)Math.Pow(magnification, Level - 1) * PoetBuff;
            }

            _currentRecastTimeCount += Time.fixedDeltaTime;
        }
    }
}