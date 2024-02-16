using System.Collections.Generic;
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

        public void InitializeObject()
        {
            //- 仮処理
            //- get npc bought count etc...

            //- ↓
            foreach (var npc in npcList)
            {
                // calculate the bought count link to npc-name
                npcShoppedInfo.Add(npc.name, 1);
            }
            //-
        }

        public void FinalizeObject()
        {
            throw new System.NotImplementedException();
        }

        void Buy(string npcName)
        {
            
        }
    }
}