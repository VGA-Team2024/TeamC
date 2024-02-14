using UnityEngine;

namespace TeamC
{
    /// <summary>詩人の処理</summary>
    public class Poet : NPCSuperClass
    {
        void OnEnable()
        {
            // 雇用してたらキャラクターを表示する
        }

        public void PoetBuff()
        {
            Debug.LogWarning("未実装です");
            return;
            // 全てのNPCの効果を2×購入数倍する
        }
    }
}