using UnityEngine;

namespace TeamC
{
    /// <summary>詩人の効果</summary>
    [CreateAssetMenu(fileName = "GeneratedPoetEffects", menuName = "CreateNPCEffects/CreatePoetEffects")]
    public class PoetEffects : ScriptableObject
    {
        private Poet _poet;
        private int _poetLevel;

        /// <summary>詩人の効果発動時の処理</summary>
        public void OnPoetEffects()
        {
            //throw new NotImplementedException();

            // 詩人の検索
            if (_poet == null)
                _poet = FindFirstObjectByType<Poet>();

            int currentLevel = _poet.GetEffectMagnification();

            if (currentLevel != _poetLevel)
            {
                // 購入数をセット
                _poet.SetEffectMagnification(_poet.GetCurrentLevel());
                _poetLevel = currentLevel;

#if UNITY_EDITOR
                Debug.Log($"現在詩人はNPCの効果を{_poet.GetEffectMagnification()}倍しています");
#endif
            }
        }
    }
}
