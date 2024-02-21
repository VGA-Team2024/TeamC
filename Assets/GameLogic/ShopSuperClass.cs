using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TeamC
{
    /// *MEMO*
    /// ～セーブデータ～
    /// NPCの購入数をセーブデータに記憶
    /// 
    /// <summary> ShopのSuperクラス </summary>
    public class ShopSuperClass : MonoBehaviour, IShop, IInitializedTarget
    {
        [SerializeField, Tooltip("購入可能なNPCのリスト"), Header("The Buyable NPCs")]
        private List<NPCDataTemplate> npcList;

        /// <summary> NPCの名前に対応したそのNPCの購入数を格納 </summary>
        private Dictionary<string, int> _npcShopHistory;

        /// <summary> NPCの購入情報NPC名とその購入数を格納している #sd </summary>
        protected Dictionary<string, int> GetNPCShopHistory
        {
            get { return _npcShopHistory; }
        }

        public void InitializeObject()
        {
            //- 仮処理
            //- get npc bought count from save-data etc...

            //- ↓
            foreach (var npc in npcList)
            {
                // calculate the bought count link to npc-name
                _npcShopHistory.Add(npc.name, 0);
            }

            //-
            throw new System.NotImplementedException();
        }

        public void FinalizeObject()
        {
            // save shop info etc...

            throw new System.NotImplementedException();
        }

        /// <summary> 購入処理。プレイヤーのリソースを減らす処理と、購入数の＋１のみ </summary>
        protected void DecreasePlayerSource(string npcName, decimal cost , Action<int> taskToInstantiate)
        {
            // get player
            var player = GameObject.FindFirstObjectByType<PlayerSuperClass>();
            // process buying 
            // get bought count
            var boughtCnt = _npcShopHistory[npcName];
            player.DecreasePlayerGold(cost);
            // task to instantiate
            taskToInstantiate(boughtCnt);
            // increment bought count 
            ++_npcShopHistory[npcName];
        }

        /// <summary> NPC名に応じたNPCの購入数に応じた価格を算出して返す </summary>
        protected decimal CalculateCostToBuy(string npcName)
        {
            // get bought count
            var boughtCnt = _npcShopHistory[npcName];
            // get target npc
            var target = npcList.Where(_ => _.Name == npcName).ToList();

            decimal cost = 0;
            // get base-price
            if (target.Count == 1) cost = (decimal)target[0].BasePrice;
            // calculate cost
            for (int i = 0; i < boughtCnt; i++)
            {
                cost *= (decimal)1.15;
            }

            return cost;
        }
    }
}