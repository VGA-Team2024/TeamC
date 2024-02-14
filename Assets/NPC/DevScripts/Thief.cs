using UnityEngine;

namespace TeamC
{
    /// <summary>盗賊の処理</summary>
    public class Thief : NPCSuperClass
    {
        [SerializeField, Header("攻撃のリキャストタイム")] private float recastTime = 10.0f;
        
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
                // 10秒に1回ボスから獲得できるゴールドの1/10000のレベル倍を入手
                // CurrentGold += (BossDropGold * ThiefLevel / 100000) * PoetBuff;
            }

            _currentRecastTimeCount += Time.fixedDeltaTime;
        }
    }
}
