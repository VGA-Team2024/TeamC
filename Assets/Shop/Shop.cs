using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SgLibUnite.Singleton;

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
    public class Shop : SingletonBaseClass<Shop>, IShop, IInitializedTarget
    {
        [SerializeField] private TMP_Text[] _npcLabels;

        [SerializeField] private Button[] _shopButton;

        private Player _player = new();
        
        [SerializeField, Tooltip("購入可能なNPCのリスト"), Header("The Buyable NPCs")]
        private List<NPCDataTemplate> npcList;

        /// <summary> NPCの名前に対応したそのNPCの購入数を格納 TName, TBoughtCount </summary>
        protected Dictionary<string, int> _npcShopHistory = new Dictionary<string, int>(); // name, bought-count

        public void InitializeObject()
        {
            var data = GameObject.FindFirstObjectByType<ClientDataSaverSuperClass>().ReadData();
            if (data == null)
            {
                foreach (var npc in npcList)
                {
                    // calculate the bought count link to npc-name
                    _npcShopHistory.Add(npc.name, 0);
                }
            }

            // get each npc level to calculate bought-count
            var wror = data._saveWarriorLevel - 1;
            var wzrd = data._saveWizardLevel - 1;
            var thf = data._saveThiefLevel - 1;
            var hrmt = data._saveHermitLevel - 1;
            var pt = data._savePoetLevel - 1;

            this._npcShopHistory.Add("Warrior", wror);
            this._npcShopHistory.Add("Wizard", wzrd);
            this._npcShopHistory.Add("Thief", thf);
            this._npcShopHistory.Add("Hermit", hrmt);
            this._npcShopHistory.Add("Poet", pt);
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
            var player = GameObject.FindFirstObjectByType<PlayerSuperClass>();
            // process buying 
            // get bought count
            var boughtCnt = this._npcShopHistory[npcName];
            player.DecreasePlayerGold(cost);
            // task to instantiate
            taskToInstantiate(boughtCnt + 1, npcName);
            // increment bought count 
            ++this._npcShopHistory[npcName];
        }

        /// <summary> NPC名に応じたNPCの購入数に応じた価格を算出して返す </summary>
        public decimal CalculateCostToBuy(string npcName)
        {
            if (_npcShopHistory.Count == 0) return 1;
            // get bought count
            var boughtCnt = 0;
            _npcShopHistory.TryGetValue(npcName, out boughtCnt);
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
        
        void TaskToInstantiateNPC(int boughtCnt, string name)
        {
            switch (name)
            {
                case "Warrior":
                    GameObject.FindFirstObjectByType<Warrior>().SetActivation((boughtCnt >= 1));
                    break;
                case "Wizard":
                    GameObject.FindFirstObjectByType<Wizard>().SetActivation((boughtCnt >= 1));
                    break;
                case "Thief":
                    GameObject.FindFirstObjectByType<Thief>().SetActivation((boughtCnt >= 1));
                    break;
                case "Hermit":
                    GameObject.FindFirstObjectByType<Hermit>().SetActivation((boughtCnt >= 1));
                    break;
                case "Poet":
                    GameObject.FindFirstObjectByType<Poet>().SetActivation((boughtCnt >= 1));
                    break;
            }
        }

        /// <summary> ボタンからNPC名を渡して購入時にこれをボタンから呼び出す </summary>
        public void BuyNPC(string name)
        {
            if (_player.GetCurrentGold() >= CalculateNPCCost(name))
            {
                // Superクラスでは、購入数に応じてコストを算出し、
                // それをプレイヤーへコストの適応をして、購入数を＋１しただけ
                DecreasePlayerSource(name, CalculateNPCCost(name), TaskToInstantiateNPC);
                UpdateButtonDisplayInfo(name);
            }
        }

        void UpdateButtonDisplayInfo(string name)
        {
            var index = name switch { "Warrior" => 0, "Wizard" => 1, "Thief" => 2, "Hermit" => 3, "Poet" => 4 };
            _shopButton[index].GetComponentInChildren<TMP_Text>().text =
                index switch { 0 => "戦士", 1 => "魔法使い", 2 => "盗賊", 3 => "仙人", 4 => "詩人" } 
                + CalculateCostToBuy(
                    index switch { 0 => "Warrior", 1 => "Wizard", 2 => "Thief", 3 => "Hermit", 4 => "Poet" } 
                ) + "G";
        }

        void UpdateButtonDisplayInfo()
        {
            //テキストの更新
            for (int i = 0; i < _shopButton.Length; i++)
            {
                _shopButton[i].GetComponentInChildren<TMP_Text>().text =
                    i switch { 0 => "戦士", 1 => "魔法使い", 2 => "盗賊", 3 => "仙人", 4 => "詩人" } 
                    + CalculateCostToBuy(
                        i switch { 0 => "Warrior", 1 => "Wizard", 2 => "Thief", 3 => "Hermit", 4 => "Poet" } 
                        ) + "G";
            }
        }

        /// <summary> NPCの購入数に応じた価格の算出 </summary>
        public decimal CalculateNPCCost(string npcName)
        {
            return CalculateCostToBuy(npcName);
        }
        
        protected override void ToDoAtAwakeSingleton()
        {
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            UpdateButtonDisplayInfo();
            Debug.Log($"Cnt:{_npcShopHistory.Count}");
        }
    }
}