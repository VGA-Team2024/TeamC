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
        private Dictionary<string, int> npcShoppedInfo;

        /// <summary> NPCの購入情報NPC名とその購入数を格納している </summary>
        protected Dictionary<string, int> GetNPCShoppedInfo
        {
            get { return npcShoppedInfo; }
        }

        public void InitializeObject()
        {
            //- 仮処理
            //- get npc bought count etc...

            //- ↓
            foreach (var npc in npcList)
            {
                // calculate the bought count link to npc-name
                npcShoppedInfo.Add(npc.name, 0);
            }
            //-
        }

        public void FinalizeObject()
        {
            // save shop info etc...

            throw new System.NotImplementedException();
        }

        /// <summary> 購入処理。プレイヤーのリソースを減らす処理と、購入数の＋１のみ </summary>
        protected void DecreasePlayerSource(string npcName, Action<int> TaskToInstanciate)
        {
            // get player
            var player = GameObject.FindFirstObjectByType<PlayerSuperClass>();
            /// process buying 

            // get bought count
            var boughtCnt = npcShoppedInfo[npcName];
            // get target npc
            var target = npcList.Where(_ => _.Name == npcName).ToList();
            
            decimal cost = 999999;
            // get base-price
            if (target.Count == 1) cost = (decimal)target[0].BasePrice;
            // calculate cost
            for (int i = 0; i < boughtCnt; i++)
                cost *= (decimal)1.15;
            // apply decrease resource to player
            player.DecreasePlayerResource(cost);
            // task to instanciate
            TaskToInstanciate(boughtCnt);
            // increment bought count 
            ++npcShoppedInfo[name];
        }
    }
}