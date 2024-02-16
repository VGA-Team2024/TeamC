using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    /// <summary> ショップ機能を提供する </summary>
    public class Shop : ShopSuperClass
    {
        void TaskToInstanciateNPC(int boughtCnt)
        {
            // ★NPCをシーン場へInstanciateする処理はSuperクラスには書いていない★
            
        }

        /// <summary> ボタンからNPC名を渡して購入時にこれをボタンから呼び出す </summary>
        public void BuyNPC(string name)
        {
            // Superクラスでは、購入数に応じてコストを算出し、それをプレイヤーへコストの適応をして、購入数を＋１しただけ
            base.DecreasePlayerSource(name, TaskToInstanciateNPC);
        }
    }
}
