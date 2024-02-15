using System.Collections;
using UnityEngine;

namespace TeamC
{
    /// <summary>仙人の処理</summary>
    public class Hermit : NPCSuperClass
    {
        [SerializeField] private long[] necessaryLevels;
        [SerializeField] private bool[] canUseSkillFlags;

        private long _level = 1;    // デバッグ用レベル
        
        void OnEnable()
        {
            // 雇用してたらキャラクターを表示する
            // 必要レベルに達しているかのチェックをする
            StartCoroutine(CheckLevel());
        }

        /// <summary>必要レベルに達しているかのチェックをする</summary>
        private IEnumerator CheckLevel()
        {
            int i = 0;

            // 雇用とレベルアップでスキルを使用可能になる
            while (true)
            {
                Debug.LogWarning("未実装です");
                break;
                
                if (_level >= necessaryLevels[i])
                {
                    canUseSkillFlags[i] = true;
                }

                i = i < necessaryLevels.Length ? i + 1 : 0;
                yield return null;
            }
        }
    }
}