using UnityEngine;

namespace TeamC
{
    /// <summary>詩人の効果</summary>
    [CreateAssetMenu(fileName = "GeneratedPoetEffects", menuName = "CreatePoetEffects")]
    public class PoetEffects : ScriptableObject
    {
        private Poet _poet;

#if UNITY_EDITOR
        private int _poetLevel;
#endif

        /// <summary>詩人の効果発動時の処理</summary>
        public void OnPoetEffects()
        {
            //throw new NotImplementedException();

            // 詩人の検索
            if (_poet == null)
                _poet = FindFirstObjectByType<Poet>();

            // 購入数をセット
            _poet.SetEffectMagnification(_poet.GetCurrentLevel());

#if UNITY_EDITOR
            int currentLevel = _poet.GetEffectMagnification();

            if (currentLevel != _poetLevel)
            {
                Debug.Log($"現在詩人はNPCの効果を{_poet.GetEffectMagnification()}倍しています");
                _poetLevel = currentLevel;
            }
#endif
        }
    }
}
