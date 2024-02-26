using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using SgLibUnite.Singleton;

namespace TeamC
{
    /// *MEMO*
    /// ～セーブデータ～
    /// NPCの購入数をセーブデータに記憶
    /// 
    /// <summary> ShopのSuperクラス </summary>
    public class ShopSuperClassL : SingletonBaseClass<ShopSuperClassL>
    {
        [SerializeField, Tooltip("購入可能なNPCのリスト"), Header("The Buyable NPCs")]
        private List<NPCDataTemplate> npcList;

        /// <summary> NPCの名前に対応したそのNPCの購入数を格納 TName, TBoughtCount </summary>
        private Dictionary<string, int> _npcShopHistory; // name, bought-count

        /// <summary> NPCの購入情報NPC名とその購入数を格納している #sd </summary>
        protected Dictionary<string, int> GetNPCShopHistory
        {
            get { return _npcShopHistory; }
        }

        public void InitializeObject()
        {
            _npcShopHistory.Clear();

            try
            {
                ClientDataTemplate dataMayDntExist =
                    GameObject.FindFirstObjectByType<ClientDataSaverSuperClass>().ReadData();
            }
            catch (FileNotFoundException)
            {
                foreach (var npc in npcList)
                {
                    // calculate the bought count link to npc-name
                    _npcShopHistory.Add(npc.name, 0);
                }

                Debug.Log("Shop-S-Class:SaveData Cannot Read!");
            }

            var data = GameObject.FindFirstObjectByType<ClientDataSaverSuperClass>().ReadData();

            // get each npc level to calculate bought-count
            var wror = data._saveWarriorLevel;
            var wzrd = data._saveWizardLevel;
            var thf = data._saveThiefLevel;
            var hrmt = data._saveHermitLevel;
            var pt = data._savePoetLevel;


            _npcShopHistory.Add("Warrior", wror);
            _npcShopHistory.Add("Wizard", wzrd);
            _npcShopHistory.Add("Thief", thf);
            _npcShopHistory.Add("Hermit", hrmt);
            _npcShopHistory.Add("Poet", pt);
        }

        public void PauseObject()
        {
            throw new NotImplementedException();
        }

        public void ResumeObject()
        {
            throw new NotImplementedException();
        }

        public void FinalizeObject()
        {
            // save shop info etc...
        }

        /// <summary> 購入処理。プレイヤーのリソースを減らす処理と、購入数の＋１処理のみ </summary>
        public void DecreasePlayerSource(string npcName, decimal cost, Action<int, string> taskToInstantiate)
        {
            // get player
            var player = GameObject.FindFirstObjectByType<PlayerSuperClassL>();
            // process buying 
            // get bought count
            var boughtCnt = _npcShopHistory[npcName];
            player.DecreasePlayerGold(cost);
            // task to instantiate
            taskToInstantiate(boughtCnt + 1, npcName);
            // increment bought count 
            ++_npcShopHistory[npcName];
        }

        /// <summary> NPC名に応じたNPCの購入数に応じた価格を算出して返す </summary>
        public decimal CalculateCostToBuy(string npcName)
        {
            var boughtCnt = 0;
            // get bought count
            if (_npcShopHistory[npcName] != null)
            {
                boughtCnt = _npcShopHistory[npcName];
            }
            else
            {
                boughtCnt = 0;
            }

            // get target npc
            var target = npcList.Where(_ => _.DisplayName == npcName).ToList();

            decimal cost = 0;
            // get base-price
            if (target.Count == 1) cost = (decimal)target[0].BasePrice;
            // calculate cost
            for (int i = 0; i <= boughtCnt; i++)
            {
                cost *= (decimal)1.15;
            }

            return cost;
        }

        protected override void ToDoAtAwakeSingleton()
        {
        }
    }
}