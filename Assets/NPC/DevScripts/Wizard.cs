using UnityEngine;

namespace TeamC
{
    /// <summary>魔法使いの処理</summary>
    public class Wizard : NPCSuperClass
    {
        void OnEnable()
        {
            // 雇用してたらキャラクターを表示する
        }

        public void WizardBuff()
        {
            Debug.LogWarning("未実装です");
            return;
            // プレイヤーのダメージを増加させる1×1.25のレベル-1乗増加
            // PlayerAttack *= (decimal)Math.Pow(1.25, Level - 1) * PoetBuff;
        }
    }
}