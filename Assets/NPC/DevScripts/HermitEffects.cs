using UnityEngine;

namespace TeamC
{
    /// <summary>仙人の効果</summary>
    [CreateAssetMenu(fileName = "GeneratedHermitEffects", menuName = "CreateNPCEffects/CreateHermitEffects")]
    public class HermitEffects : ScriptableObject
    {
        [SerializeField, Header("スキルのデータベース")] private SkillsDataTemplate[] skills;

        private Hermit _hermit;
        private int _hermitLevel;
        
        /// <summary>仙人の効果発動時の処理</summary>
        public void OnHermitEffects()
        {
            //throw new NotImplementedException();

            // 仙人の検索
            if (_hermit == null)
                _hermit = FindFirstObjectByType<Hermit>();

            // 現在の仙人のレベルを取得
            int currentLevel = _hermit.GetCurrentLevel();

            // 記録されているレベルが違えば
            if (_hermitLevel != currentLevel)
            {
                // 雇用とレベルアップでスキルを使用可能になる
                foreach (var skill in skills)
                {
                    if (currentLevel >= skill.RequiredLevel)
                    {
                        // スキルのロックが解除される
                        skill.IsLocked = false;

#if UNITY_EDITOR
                        Debug.Log($"{skill.name}が使用可能です");
#endif
                    }
                    else
                    {
                        // スキルをロックする
                        skill.IsLocked = true;

#if UNITY_EDITOR
                        Debug.Log($"{skill.name}は使用不可能です");
#endif
                    }
                }

                // レベルの記録
                _hermitLevel = currentLevel;
            }
        }
    }
}
