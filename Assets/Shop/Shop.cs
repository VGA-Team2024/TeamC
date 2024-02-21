using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    #region TODO

    // 2/17 菅沼 → 川口
    // SuperClass ではモジュールのInitialize処理とFinalize処理と購入処理の一部しか
    // 実装していない。
    // Init処理とFinal処理ではClientDataの読み書き処理を実装する予定である。
    // 購入処理ではNPC名に対応するNPCの購入数に応じた価格をプレイヤーのリソースから
    // 減算してTaskToInstantiateNPC()を呼び出してNPCの購入数を＋１するだけの処理を実装している
    // 実装してほしいのがあれば逐次教えてください

    #endregion

    /// <summary> ショップ機能を提供する </summary>
    public class Shop : ShopSuperClass
    {
        void TaskToInstantiateNPC(int boughtCnt)
        {
            // ★NPCをシーン場へInstanciateする処理はSuperクラスには書いていない★
        }

        /// <summary> ボタンからNPC名を渡して購入時にこれをボタンから呼び出す </summary>
        public void BuyNPC(string name)
        {
            // Superクラスでは、購入数に応じてコストを算出し、
            // それをプレイヤーへコストの適応をして、購入数を＋１しただけ
            base.DecreasePlayerSource(name, CalculateNPCCost(name), TaskToInstantiateNPC);
        }

        /// <summary> NPCの購入数に応じた価格の算出 </summary>
        public decimal CalculateNPCCost(string npcName) => base.CalculateCostToBuy(npcName);
    }
}