using System;
using UnityEngine;

namespace TeamC
{
    /// <summary>仙人の効果</summary>
    [CreateAssetMenu(fileName = "GeneratedHermitEffects", menuName = "CreateHermitEffects")]
    public class HermitEffects : ScriptableObject
    {
        [SerializeField, Header("スキルのデータベース")] private SkillsSuperClass[] skills;

        private Hermit _hermit;
        private int _hermitLevel;

        /// <summary>仙人の効果発動時の処理</summary>
        public void OnHermitEffects()
        {
            // 仙人の検索
            if (_hermit == null)
                _hermit = FindFirstObjectByType<Hermit>();

            // 現在の仙人のレベルを取得
            int currentLevel = _hermit.GetCurrentLevel();

            // 記録されているレベルが違えば
            if (_hermitLevel != currentLevel)
            {
                throw new NotImplementedException();

                // 雇用とレベルアップでスキルを使用可能になる
                foreach (var skill in skills)
                {
                    // if (skill.GetSkillData.RequiredLevel >= currentLevel)
                    // {
                    //     // スキルのロックが解除される
                    // }
                }

                // レベルの記録
                _hermitLevel = currentLevel;
            }
        }
    }
}
